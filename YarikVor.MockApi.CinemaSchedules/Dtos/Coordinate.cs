using System.Globalization;

namespace YarikVor.MockApi.CinemaSchedules;

public struct Coordinate
{
    public double Longitude { get; }
    public double Latitude { get; }

    public Coordinate(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    public override string ToString()
    {
        var invariantCulture = CultureInfo.InvariantCulture;
        return $"{Longitude.ToString(invariantCulture)}, {Latitude.ToString(invariantCulture)}";
    }
}