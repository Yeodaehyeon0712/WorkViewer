using Firebase.Extensions;
using RobinBird.FirebaseTools.Storage.Addressables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AddressableSystem
{
    eAddressableState state;
    public bool IsLoad;
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
                DataManager.Instance.StartCoroutine(IELoad());
            }
            else
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        });
    }

    #region Addressable Method
    IEnumerator IELoad()
    {
        state = eAddressableState.FindPatch;
        var cachePaths = new List<string>();
        Caching.GetAllCachePaths(cachePaths);
        //foreach (var cachePath in cachePaths)
        //{
        //    EditorDebug.Log($"Cache path: {cachePath}");
        //}

        // Caching.ClearCache();

        yield return IECatalogCheck();
        while (state == eAddressableState.FindPatch)
            yield return null;

        if (state != eAddressableState.LoadMemory)
            yield return IEDownloadAsset();
        else
            yield return IELoadMemory();
        IsLoad = true;
    }
    IEnumerator IECatalogCheck()
    {
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates();
        yield return checkForUpdateHandle;
        List<string> catalogsToUpdate = checkForUpdateHandle.Result;
        if (catalogsToUpdate.Count > 0)
        {
            Debug.LogWarning("UpdateCatalogsStart");
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, true);
            yield return updateHandle;
            Debug.LogWarning("UpdateCatalogsEnd");
            state = eAddressableState.FoundSize;
        }
        else
            state = eAddressableState.LoadMemory;
        yield return null;
        Addressables.Release(checkForUpdateHandle);
    }
    IEnumerator IEDownloadAsset()
    {
        Debug.Log("-- 어드레서블 DownloadAsset --");

        state = eAddressableState.FoundSize;
        AsyncOperationHandle operationHandle = Addressables.DownloadDependenciesAsync("default");
        state = eAddressableState.Downloading;
        yield return operationHandle;
        AssetBundle.UnloadAllAssetBundles(true);
        Addressables.Release(operationHandle);
        yield return IELoadMemory();
    }
    IEnumerator IELoadMemory()
    {
        state = eAddressableState.LoadMemory;
        AsyncOperationHandle<IResourceLocator> locatorHandle = Addressables.InitializeAsync(true);
        yield return locatorHandle;

        yield return IELoadModelMemory();
    }
    Dictionary<string, GameObject> _modelContainer = new Dictionary<string, GameObject>();
    IEnumerator IELoadModelMemory()
    {
        state = eAddressableState.IconMemory;

        AsyncOperationHandle<IList<IResourceLocation>> locationList = Addressables.LoadResourceLocationsAsync("Model", null);
        yield return null;
        int taskCount = 0;
        int jobCount = 0;
        foreach (var location in locationList.Result)
        {
            if (location.ResourceType != typeof(GameObject))
            {
                ++jobCount;
                yield return null;
                continue;
            }


            ++taskCount;
            Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey).Completed += task =>
            {
                _modelContainer.Add(location.PrimaryKey, task.Result);
                --taskCount;
                ++jobCount;
                Debug.Log(task.Result.name);
            };
        }
        while (taskCount > 0)
            yield return null;
        Addressables.Release(locationList);
    }
    #endregion

}
enum eAddressableState
{
    Initialize,
    FindPatch,
    FoundSize,
    Downloading,
    LoadMemory,
    Complete,
    IconMemory,
    SpriteMemory,
    SpineGUI,
    Effect,
    Quest,
    Background,
    Network,
    IAP,
    AdMob,
    Notifications,
    AppsFlyer,

}
