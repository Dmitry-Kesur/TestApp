using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IPreloaderService
    {
        Action<float, string> UpdateLoadingProgressAction { set; }
        Task Load();
    }
}