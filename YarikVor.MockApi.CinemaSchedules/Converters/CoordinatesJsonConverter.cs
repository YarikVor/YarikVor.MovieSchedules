using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YarikVor.MockApi.CinemaSchedules.Converters;

public class CoordinatesJsonConverter : JsonConverter<Coordinate>
{
    public override Coordinate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);

        var array = document.Deserialize<string[]>()
            .Select(e => double.Parse(e, CultureInfo.InvariantCulture))
            .ToArray();

        return new Coordinate(array[0], array[1]);
    }

    public override void Write(Utf8JsonWriter writer, Coordinate value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}