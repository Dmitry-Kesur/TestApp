using System.Threading.Tasks;

public interface IPlayerProgressService
{
    Task LoadPlayerProgress(string userId);
    void SavePlayerProgress();
}