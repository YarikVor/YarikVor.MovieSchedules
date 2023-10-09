namespace YarikVor.KinoSchedules;

public readonly struct TimeRange
{
    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public TimeSpan Duration => End - Start;

    public TimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        Start = start;
        End = end;
    }

    public TimeRange(DateTimeOffset start, TimeSpan duration) : this(start, start + duration)
    {
    }

    public bool IsInRange(TimeRange timeRange)
    {
        return timeRange.Start >= Start && timeRange.End <= End;
    }
}