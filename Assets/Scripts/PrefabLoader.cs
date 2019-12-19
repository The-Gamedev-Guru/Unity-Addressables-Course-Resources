using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace TheGamedevGuru
{
public class PrefabLoader : MonoBehaviour
{
    [SerializeField] private AssetReference prefabReference = null;
    
    private IEnumerator DemoAddressablePrefab()
    {
        var asyncOperationHandle = prefabReference.LoadAssetAsync<GameObject>();
        yield return asyncOperationHandle;
        var prefab = asyncOperationHandle.Result;
        var gameObjects = new List<GameObject>();
        for (var i = 0; i < 10; i++)
        {
            gameObjects.Add(Instantiate(prefab, Vector3.right * i, Quaternion.identity));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        for (var i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
            yield return new WaitForSeconds(0.1f);
        }
        Addressables.Release(asyncOperationHandle);
    }

    IEnumerator Start()
    {
        yield return DemoAddressablePrefab();
    }
}
}
