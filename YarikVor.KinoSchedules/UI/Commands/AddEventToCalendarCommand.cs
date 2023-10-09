using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using YarikVor.MockApi.CinemaSchedules;

namespace YarikVor.KinoSchedules;

public class AddEventToCalendarCommand : ICommand
{
    private readonly CalendarService _calendarService;
    private readonly EventsResource.ListRequest _listRequest;
    private readonly Navigation _navigation;
    private readonly IMovieScheduleClient _scheduleClient;

    public AddEventToCalendarCommand(
        Navigation navigation,
        IMovieScheduleClient scheduleClient,
        CalendarService calendarService)
    {
        _navigation = navigation;
        _scheduleClient = scheduleClient;
        _calendarService = calendarService;
        _listRequest = _calendarService.Events.List("primary");
    }

    public async Task InvokeAsync(params object[] parameters)
    {
        if (parameters[0] is MovieDto movieDto)
            await InvokeAsync(movieDto);

        _navigation.Pop();
    }

    private async Task InvokeAsync(MovieDto movieDto)
    {
        var utcNow = DateTimeOffset.UtcNow;

        var schedules = await _scheduleClient
            .GetSchedulesAsync(movieDto.Id);

        var schedulesTimeRange = schedules
            .OrderBy(e => e.StartAt)
            .Select(s => new TimeRange(s.StartAt, TimeSpan.FromSeconds(movieDto.Duration)))
            .ToArray();

        var lastSchedule = schedulesTimeRange.Last();
        var lastTime = lastSchedule.Start + TimeSpan.FromSeconds(movieDto.Duration);

        var freeTimeRanges = await GetFreeTimeRanges(new TimeRange(utcNow, lastTime));

        var arrayCalculationFilmTime = CalculationTimes
            .GetFreeTime(freeTimeRanges, schedulesTimeRange)
            .ToArray();

        Console.Write("Name: ");
        Console.WriteLine(movieDto.Name);
        Console.Write("Description: ");
        Console.WriteLine(movieDto.Description);

        Console.WriteLine("Please select index for write in Google Calendar: ");

        var currentUtfOffset = DateTimeOffset.Now.Offset;
        for (var index = 0; index < arrayCalculationFilmTime.Length; index++)
        {
            var timeRange = arrayCalculationFilmTime[index];
            var currentTimeRange = new TimeRange(
                timeRange.Start.ToOffset(currentUtfOffset),
                timeRange.End.ToOffset(currentUtfOffset)
            );

            Console.WriteLine($"{index,-3} {currentTimeRange.Start:g} - {currentTimeRange.End:t}");
        }

        var selectedIndex = ConsoleMethods.InputInt32("Input index (-1 - skip): ");

        if (selectedIndex >= 0 && selectedIndex < arrayCalculationFilmTime.Length)
        {
            var timeRange = arrayCalculationFilmTime[selectedIndex];
            var schedule = schedules[selectedIndex];
            var newEvent = CreateEvent(timeRange, movieDto, schedule);
            var result = await SendEventAsync(newEvent);
            Console.WriteLine("Event added");
            Console.WriteLine($"Link: {result.HtmlLink}");
        }

        ConsoleMethods.Pause();
    }

    private async Task<Event> SendEventAsync(Event newEvent)
    {
        return await _calendarService.Events
            .Insert(newEvent, "primary")
            .ExecuteAsync();
    }


    private static Event CreateEvent(TimeRange timeRange, MovieDto movieDto, ScheduleDto schedule)
    {
        return new Event
        {
            Start = EventDateTimeMethods.FromDateTimeOffset(timeRange.Start),
            End = EventDateTimeMethods.FromDateTimeOffset(timeRange.End),
            Summary = movieDto.Name,
            Description = movieDto.Description,
            Location = schedule.Coordinate.ToString()
        };
    }

    private async Task<IEnumerable<TimeRange>> GetFreeTimeRanges(TimeRange timeRange)
    {
        var events = await _listRequest.ExecuteAsync();
        var ls = new ListTimeRange(timeRange);
        
        var timeRanges = events.Items
            .Select(e => new TimeRange(
                    DateTimeOffset.Parse(e.Start.DateTimeRaw).UtcDateTime,
                    DateTimeOffset.Parse(e.End.DateTimeRaw).UtcDateTime
                )
            )
            .Where(e => e.End >= timeRange.Start)
            .OrderBy(e => e.Start);

        foreach (var item in timeRanges)
            ls.Sub(item);

        return ls.TimeRanges;
    }
}