using System.Text.Json.Serialization;
using YarikVor.MockApi.CinemaSchedules.Converters;

namespace YarikVor.MockApi.CinemaSchedules;

public class ScheduleDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public DateTimeOffset StartAt { get; set; }

    [JsonConverter(typeof(CoordinatesJsonConverter))]
    public Coordinate Coordinate { get; set; }
}