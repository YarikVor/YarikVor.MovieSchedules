namespace YarikVor.KinoSchedules;

public class NavigationItem
{
    private readonly ICommand _command;
    private readonly object[] _parameters;

    public NavigationItem(ICommand command, object[] parameters)
    {
        _command = command;
        _parameters = parameters;
    }

    public async Task InvokeAsync()
    {
        await _command.InvokeAsync(_parameters);
    }
}