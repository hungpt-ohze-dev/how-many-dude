using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : LiveSingleton<AddressableManager>
{
    private Dictionary<string, AsyncOperationHandle<GameObject>> prefabHandles = new();

    private Dictionary<string, List<GameObject>> spawnedInstances = new();

    //private Dictionary<string, LevelSO> levelConfigs = new();

    // Loads a prefab (if not cached), instantiates it, and returns the instance.
    public async UniTask<GameObject> LoadAndInstantiateAsync(string prefabAddress)
    {
        GameObject prefab;

        if (prefabHandles.ContainsKey(prefabAddress))
        {
            // Already cached
            prefab = prefabHandles[prefabAddress].Result;
        }
        else
        {
            // Load prefab asynchronously
            var handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new System.Exception($"Failed to load prefab: {prefabAddress}");

            prefab = handle.Result;
            prefabHandles[prefabAddress] = handle;
        }

        // Instantiate and track instance
        GameObject instance = Instantiate(prefab);

        if (!spawnedInstances.ContainsKey(prefabAddress))
            spawnedInstances[prefabAddress] = new List<GameObject>();

        spawnedInstances[prefabAddress].Add(instance);

        return instance;
    }

    // Release a specific instance.
    public void ReleaseInstance(string prefabAddress, GameObject instance)
    {
        if (spawnedInstances.ContainsKey(prefabAddress) &&
            spawnedInstances[prefabAddress].Contains(instance))
        {
            spawnedInstances[prefabAddress].Remove(instance);
            Destroy(instance);
        }
    }

    // Release all instances of a prefab.
    public void ReleaseAllInstances(string prefabAddress)
    {
        if (spawnedInstances.ContainsKey(prefabAddress))
        {
            foreach (var inst in spawnedInstances[prefabAddress])
                Destroy(inst);

            spawnedInstances[prefabAddress].Clear();
        }
    }

    // Release everything (instances + prefab handles).
    public void ReleaseAll()
    {
        foreach (var kvp in spawnedInstances)
        {
            foreach (var inst in kvp.Value)
                Destroy(inst);
        }
        spawnedInstances.Clear();

        foreach (var kvp in prefabHandles)
        {
            Addressables.Release(kvp.Value);
        }
        prefabHandles.Clear();
    }

    // Level
    //public async UniTask<LevelController> LoadLevel(int id)
    //{
    //    string prefabAsset = $"Level/Level {id}.prefab";
    //    GameObject obj = await LoadAndInstantiateAsync(prefabAsset);
    //    return obj.GetComponent<LevelController>();
    //}

    // Config
    //public async void LoadAllLevelSO()
    //{
    //    string label = "Level_Config";
    //    AsyncOperationHandle<IList<LevelSO>> handle =
    //        Addressables.LoadAssetsAsync<LevelSO>(label, null); // label

    //    IList<LevelSO> items = await handle.Task;

    //    foreach (var item in items)
    //    {
    //        if (!levelConfigs.ContainsKey(item.name))
    //            levelConfigs.Add(item.name, item);
    //    }

    //    Debug.Log("Loaded item count: " + levelConfigs.Count);
    //}

    //public LevelSO GetLevelConfig(int id)
    //{
    //    string levelId = $"Level {id}";
    //    levelConfigs.TryGetValue(levelId, out LevelSO data);
    //    return data;
    //}
}
