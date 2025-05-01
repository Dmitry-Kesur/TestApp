using UnityEngine;

namespace Infrastructure.Services
{
    public class PrefabInstantiationService
    {
        public T GetPrefabInstance<T>(string prefabPath) where T : Object
        {
            var prefab = Resources.Load<T>(prefabPath);
            var prefabInstance = Object.Instantiate(prefab);
            return prefabInstance;
        }
    }
}