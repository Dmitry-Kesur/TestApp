using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Utils
{
    public class PrefabInstantiationService : IPrefabInstantiationService
    {
        public T GetPrefabInstance<T>(string prefabPath) where T : Object
        {
            var prefab = Resources.Load<T>(prefabPath);
            var prefabInstance = Object.Instantiate(prefab);
            return prefabInstance;
        }
    }
}