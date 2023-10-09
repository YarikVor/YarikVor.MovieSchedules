using Google.Apis.Calendar.v3.Data;

namespace YarikVor.KinoSchedules;

public static class EventDateTimeMethods
{
    public static EventDateTime FromDateTimeOffset(DateTimeOffset dateTimeOffset)
    {
        return new EventDateTime { DateTimeDateTimeOffset = dateTimeOffset };
    }
}