using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Preloader
{
    public interface IPreloaderService
    {
        Action<float, string> UpdateLoadingProgressAction { set; }
        Task Load();
    }
}