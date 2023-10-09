using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace YarikVor.KinoSchedules;

public static class CustomCalendarService
{
    private const string ApplicationName = "CinemaSchedules";

    private static readonly string[] Scopes =
    {
        CalendarService.Scope.CalendarReadonly,
        CalendarService.Scope.CalendarEvents,
        CalendarService.Scope.Calendar
    };

    public static async Task<CalendarService> GetCurrentCalendarService()
    {
        var googleClientSecrets = await GoogleClientSecrets.FromFileAsync("client_secret.json");
        var userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            googleClientSecrets.Secrets,
            Scopes,
            "user1",
            CancellationToken.None
        );
        var initializer = new BaseClientService.Initializer
        {
            HttpClientInitializer = userCredential,
            ApplicationName = ApplicationName
        };
        return new CalendarService(initializer);
    }
}