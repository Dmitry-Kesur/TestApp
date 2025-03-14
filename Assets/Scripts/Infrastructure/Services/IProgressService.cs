using System.Threading.Tasks;

public interface IProgressService
{
    Task LoadPlayerProgress(string userId);
    void SavePlayerProgress();
}