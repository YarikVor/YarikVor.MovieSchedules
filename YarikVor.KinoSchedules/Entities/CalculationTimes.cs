namespace YarikVor.KinoSchedules;

public class CalculationTimes
{
    public static IEnumerable<TimeRange> GetFreeTime(IEnumerable<TimeRange> freeTime, IEnumerable<TimeRange> stopTime)
    {
        var array = stopTime.ToList();

        foreach (var ft in freeTime)
        {
            if (array.Count == 0)
                yield break;
            for (var index = 0; index < array.Count; index++)
            {
                var sTimeRange = array[index];
                if (!ft.IsInRange(sTimeRange))
                    continue;

                array.RemoveAt(index);
                index--;
                yield return sTimeRange;
            }
        }
    }
}