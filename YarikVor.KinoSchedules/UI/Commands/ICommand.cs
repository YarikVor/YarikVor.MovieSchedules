namespace YarikVor.KinoSchedules;

public interface ICommand
{
    public Task InvokeAsync(params object[] parameters);
}