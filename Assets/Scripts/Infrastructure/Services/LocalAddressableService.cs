using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Infrastructure.Services
{
    public class LocalAddressableService
    {
        public async Task<T> InstantiatePrefab<T>(string prefabName) where T : Object
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            var instance = Addressables.InstantiateAsync(prefabName);

            await instance.Task;

            if (instance.IsDone)
            {
                var result = instance.Result.GetComponent<T>();
                taskCompletionSource.SetResult(result);
            }

            return await taskCompletionSource.Task;
        }

        public async Task<List<T>> LoadScriptableCollectionFromGroupAsync<T>(string groupKey) where T : class
        {
            TaskCompletionSource<List<T>> tcs = new TaskCompletionSource<List<T>>();

            AsyncOperationHandle<IList<IResourceLocation>> loadGroupOperation =
                Addressables.LoadResourceLocationsAsync(groupKey);

            await loadGroupOperation.Task;

            var groupOperationResult = loadGroupOperation.Result;

            List<T> dataList = new List<T>();

            foreach (var resourceLocation in groupOperationResult)
            {
                AsyncOperationHandle<T>
                    loadDataOperation = Addressables.LoadAssetAsync<T>(resourceLocation);

                await loadDataOperation.Task;

                var dataOperationResult = loadDataOperation.Result;
                dataList.Add(dataOperationResult);
            }

            tcs.SetResult(dataList);

            return await tcs.Task;
        }

        public void Release(GameObject gameObject)
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}