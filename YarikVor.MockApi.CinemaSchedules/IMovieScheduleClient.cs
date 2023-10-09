namespace YarikVor.MockApi.CinemaSchedules;

public interface IMovieScheduleClient
{
    Task<MovieDto[]> GetMoviesAsync();
    Task<ScheduleDto[]?> GetSchedulesAsync(int id);
}