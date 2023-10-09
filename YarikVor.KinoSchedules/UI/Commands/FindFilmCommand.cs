using YarikVor.MockApi.CinemaSchedules;

namespace YarikVor.KinoSchedules;

public class FindFilmCommand : ICommand
{
    private readonly Navigation _navigation;
    private readonly IMovieScheduleClient _scheduleClient;

    public FindFilmCommand(Navigation navigation, IMovieScheduleClient scheduleClient)
    {
        _navigation = navigation;
        _scheduleClient = scheduleClient;
    }

    public async Task InvokeAsync(params object[] parameters)
    {
        var movies = await _scheduleClient.GetMoviesAsync();

        MovieDto selectedMovie;

        while (true)
        {
            begin:
            Console.Clear();
            Console.Write("Input film's name: ");
            var filmName = Console.ReadLine();

            var index = 0;

            var moviesSelect = movies.Where(
                    e => e.Name.Contains(filmName, StringComparison.InvariantCultureIgnoreCase)
                )
                .ToArray();

            if (moviesSelect.Length == 0)
            {
                Console.WriteLine("Films not found!");
                ConsoleMethods.Pause();
                continue;
            }

            while (true)
            {
                Console.WriteLine("Please select your films");

                Console.Write(" > ");
                foreach (var movieDto in moviesSelect.Skip(index).Take(3)) 
                    Console.WriteLine(movieDto.Name);

                Console.WriteLine("Q - quit, Enter - select film");

                var keyCode = Console.ReadKey();

                if (keyCode.Key == ConsoleKey.UpArrow)
                {
                    index--;
                }
                else if (keyCode.Key == ConsoleKey.DownArrow)
                {
                    index++;
                }
                else if (keyCode.Key == ConsoleKey.Enter)
                {
                    selectedMovie = moviesSelect[index];
                    break;
                }
                else if (keyCode.Key == ConsoleKey.Q)
                {
                    goto begin;
                }

                if (index < 0)
                    index = 0;
                if (index >= moviesSelect.Length)
                    index = moviesSelect.Length - 1;

                Console.Clear();
            }

            break;
        }

        _navigation.Push<AddEventToCalendarCommand>(selectedMovie);
    }
}