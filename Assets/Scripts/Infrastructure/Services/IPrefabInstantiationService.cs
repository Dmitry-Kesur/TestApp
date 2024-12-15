using UnityEngine;

namespace Infrastructure.Services
{
    public interface IPrefabInstantiationService
    {
        T GetPrefabInstance<T>(string prefabPath) where T : Object;
    }
}