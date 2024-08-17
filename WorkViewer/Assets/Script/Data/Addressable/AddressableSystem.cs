using Firebase.Extensions;
using RobinBird.FirebaseTools.Storage.Addressables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Cysharp.Threading.Tasks;
using System;

public class AddressableSystem
{
    eAddressableState state;
    public bool IsLoad;
    Dictionary<string, GameObject> _modelContainer = new Dictionary<string, GameObject>();
    Dictionary<string, TextAsset> _tableContainer = new Dictionary<string, TextAsset>();

    public void Initialize()
    {
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageAssetBundleProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageJsonAssetProvider());
        Addressables.ResourceManager.ResourceProviders.Add(new FirebaseStorageHashProvider());
        Addressables.InternalIdTransformFunc += FirebaseAddressablesCache.IdTransformFunc;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                FirebaseAddressablesManager.IsFirebaseSetupFinished = true;
                state = eAddressableState.Initialized;
                //해당 부분은 고민이 필요함 . Data Manager에게 넘길지 아니면 그냥 처리할지 .
                LoadAsync().Forget();
                //DataManager.Instance.RunTask(LoadAsync());
            }
            else
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        });
    }

    #region Addressable Method
    async UniTask LoadAsync()
    {
        try
        {
            #region Cache Method
            //var cachePaths = new List<string>();
            //Caching.GetAllCachePaths(cachePaths);
            //foreach (var cachePath in cachePaths)
            //{
            //    Debug.Log($"Cache path: {cachePath}");
            //}
            //Caching.ClearCache();
            #endregion
            await CatalogCheckAsync();

            if (state == eAddressableState.FindPatch)
                await DownloadAssetAsync();
            else
                await LoadMemoryAsync();

            state = eAddressableState.Complete;
            IsLoad = true;
        }
        catch(Exception ex)
        {
            Debug.LogError($"An error occurred during the LoadAsync: {ex.Message}");
        }
    }
    async UniTask CatalogCheckAsync()
    {
        try
        {
            AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
            await checkForUpdateHandle.ToUniTask();

            List<string> catalogsToUpdate = checkForUpdateHandle.Result;
            if (catalogsToUpdate.Count > 0)
            {
                AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, true);
                await updateHandle.ToUniTask();
                state = eAddressableState.FindPatch;
            }
            else
                state = eAddressableState.LoadMemory;
        }
        catch(Exception ex)
        {
            Debug.LogError($"An error occurred during the catalog check: {ex.Message}");
        }   
    }

    async UniTask DownloadAssetAsync()
    {
        AsyncOperationHandle operationHandle = Addressables.DownloadDependenciesAsync("default");
        state = eAddressableState.Downloading;
        await operationHandle.ToUniTask();
        AssetBundle.UnloadAllAssetBundles(true);
        Addressables.Release(operationHandle);
        await LoadMemoryAsync();    
    }
    async UniTask LoadMemoryAsync()
    {
        state = eAddressableState.LoadMemory;
        AsyncOperationHandle<IResourceLocator> locatorHandle = Addressables.InitializeAsync(true);
        await locatorHandle.ToUniTask();

        await LoadModelMemoryAsync();
        await LoadTableMemoryAsync();

        state = eAddressableState.Complete;
    }
    async UniTask LoadModelMemoryAsync()
    {
        state = eAddressableState.ModelMemory;

        AsyncOperationHandle<IList<IResourceLocation>> locationListHandle = Addressables.LoadResourceLocationsAsync("Model", null);
        await locationListHandle.ToUniTask();

        List<UniTask> loadingTasks = new List<UniTask>();
        foreach (var location in locationListHandle.Result)
        {
            if (location.ResourceType != typeof(GameObject))
            {
                continue;
            }

            var loadTask = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey).ToUniTask().ContinueWith(task =>
            {
                  _modelContainer.Add(location.PrimaryKey, task);
             });

            loadingTasks.Add(loadTask);
        }

        await UniTask.WhenAll(loadingTasks);
        Addressables.Release(locationListHandle);
    }
    async UniTask LoadTableMemoryAsync()
    {
        state = eAddressableState.TableMemory;

        AsyncOperationHandle<IList<IResourceLocation>> locationListHandle = Addressables.LoadResourceLocationsAsync("Table", null);
        await locationListHandle.ToUniTask();

        List<UniTask> loadingTasks = new List<UniTask>();
        foreach (var location in locationListHandle.Result)
        {
            if (location.ResourceType != typeof(TextAsset))
            {
                continue;
            }

            var loadTask = Addressables.LoadAssetAsync<TextAsset>(location.PrimaryKey).ToUniTask().ContinueWith(task =>
            {
                _tableContainer.Add(location.PrimaryKey, task);
            });

            loadingTasks.Add(loadTask);
        }

        await UniTask.WhenAll(loadingTasks);
        Addressables.Release(locationListHandle);
    }
    #endregion

    #region Get
    public static GameObject GetModel(string key)
    {
        if (DataManager.AddressableSystem._modelContainer.TryGetValue(key, out var model))
        {
            return model;
        }

        Debug.LogError($"Model with resource path {key} not found in the model container.");
        return null;    
    }
    public static TextAsset GetTable(string key)
    {
        if (DataManager.AddressableSystem._tableContainer.TryGetValue(key, out var database))
        {
            return database;
        }

        Debug.LogError($"Database with resource path {key} not found in the database container.");
        return null;
    }
    #endregion

}
enum eAddressableState
{
    None,
    Initialized,
    FindPatch,
    Downloading,
    LoadMemory,
    ModelMemory,
    TableMemory,
    Complete,
}
