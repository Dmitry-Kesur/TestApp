namespace Infrastructure.Services.Bootstrap
{
    public interface IBootstrapTarget
    {
        int InitializationOrder { get; }
        void Initialize();
    }
}