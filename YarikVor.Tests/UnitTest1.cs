using YarikVor.MockApi.CinemaSchedules;

namespace YarikVor.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test()
    {
        var client = new MockMovieScheduleClient();

        var movies = await client.GetMoviesAsync();

        var shedules = await client.GetSchedulesAsync(movies[3].Id);
    }
}