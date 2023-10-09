using System.Net.Http.Json;

namespace YarikVor.MockApi.CinemaSchedules;

public class MockMovieScheduleClient : IMovieScheduleClient
{
    public const string BaseUrl = "https://6522dd89f43b17938414fbeb.mockapi.io/api/v1/";
    public static readonly Uri BaseUri = new(BaseUrl);
    private readonly HttpClient _httpClient;

    public MockMovieScheduleClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = BaseUri;
    }

    public MockMovieScheduleClient() : this(new HttpClient())
    {
    }

    public async Task<MovieDto[]> GetMoviesAsync()
    {
        return (await _httpClient.GetFromJsonAsync<MovieDto[]>("movies"))!;
    }

    public async Task<ScheduleDto[]?> GetSchedulesAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ScheduleDto[]?>($"movies/{id}/schedules");
    }
}