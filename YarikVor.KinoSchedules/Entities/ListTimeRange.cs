namespace YarikVor.KinoSchedules;

public class ListTimeRange
{
    private readonly List<TimeRange> _timeRanges;

    public ListTimeRange(TimeRange timeRange)
    {
        _timeRanges = new List<TimeRange> { timeRange };
    }

    public IEnumerable<TimeRange> TimeRanges => _timeRanges;

    public void Sub(TimeRange subTimeRange)
    {
        for (var index = 0; index < _timeRanges.Count; index++)
        {
            var timeRange = _timeRanges[index];

            if (timeRange.End < subTimeRange.Start)
                continue;

            if (timeRange.IsInRange(subTimeRange))
            {
                var timeRanges = new TimeRange[]
                {
                    new(timeRange.Start, subTimeRange.Start),
                    new(subTimeRange.End, timeRange.End)
                };

                _timeRanges.RemoveAt(index);

                var enumerableTimeRanges = timeRanges.Where(tR => tR.Duration != TimeSpan.Zero);
                _timeRanges.InsertRange(index, enumerableTimeRanges);
                return;
            }

            if (subTimeRange.IsInRange(timeRange))
            {
                _timeRanges.RemoveAt(index);
                index--;
                continue;
            }

            if (timeRange.Start <= subTimeRange.Start && subTimeRange.Start <= timeRange.End)
            {
                var newTimeRange = new TimeRange(timeRange.Start, subTimeRange.Start);
                _timeRanges[index] = newTimeRange;
                continue;
            }

            if (timeRange.Start <= subTimeRange.End && subTimeRange.End <= timeRange.End)
            {
                var newTimeRange = new TimeRange(subTimeRange.End, timeRange.End);
                _timeRanges[index] = newTimeRange;
                return;
            }
        }
    }
}