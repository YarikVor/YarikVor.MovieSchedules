using Microsoft.Extensions.DependencyInjection;

namespace YarikVor.KinoSchedules;

public class Navigation
{
    private readonly Stack<NavigationItem> _commands = new();
    private readonly IServiceProvider _services;

    public Navigation(IServiceProvider services)
    {
        _services = services;
    }

    public void Push<TNavigation>(params object[] parameters) where TNavigation : ICommand
    {
        Push(typeof(TNavigation), parameters);
    }

    public void Push(Type type, params object[] parameters)
    {
        var command = (ICommand)_services.GetRequiredService(type);
        var navigationItem = new NavigationItem(command, parameters);
        _commands.Push(navigationItem);
    }

    public void Pop()
    {
        _commands.Pop();
    }

    public async Task InvokeLastAsync()
    {
        await _commands.Peek().InvokeAsync();
    }

    public bool IsAny()
    {
        return _commands.Any();
    }
}