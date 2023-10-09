using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YarikVor.MockApi.CinemaSchedules;

namespace YarikVor.KinoSchedules;

public class ConsoleRunner
{
    private readonly Navigation _navigation;
    private readonly IServiceProvider _serviceProvider;

    public ConsoleRunner()
    {
        _serviceProvider = GetServiceProvider();
        _navigation = _serviceProvider.GetRequiredService<Navigation>();
    }

    private IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<Navigation>();
        services.AddSingleton<IMovieScheduleClient, MockMovieScheduleClient>();
        services.AddSingleton(CustomCalendarService.GetCurrentCalendarService().Result);
        services.AddSingleton(this);

        var subclassType = typeof(ICommand);
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(e => e != subclassType)
            .Where(e => e.IsAssignableTo(subclassType));

        foreach (var type in types)
            services.AddSingleton(type);

        return services.BuildServiceProvider();
    }

    public async Task RunAsync()
    {
        while (_navigation.IsAny())
        {
            Console.Clear();
            await _navigation.InvokeLastAsync();
        }
    }

    public Task InitAsync()
    {
        _navigation.Push<FindFilmCommand>();
        return Task.CompletedTask;
    }
}