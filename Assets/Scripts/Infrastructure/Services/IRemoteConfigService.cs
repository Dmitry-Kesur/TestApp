namespace Infrastructure.Services
{
    public interface IRemoteConfigService : IInitializeAsync
    {
        string GetValue(string key);
    }
}