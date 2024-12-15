namespace Infrastructure.Services
{
    public interface ICrashlyticsService : IInitializeAsync
    {
        void LogError(string message);
        void LogException(System.Exception exception);
    }
}