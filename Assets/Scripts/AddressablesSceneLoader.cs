using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class AddressablesSceneLoader : MonoBehaviour
{
    [SerializeField] private AssetReference sceneReference = null;

    IEnumerator Start()
    {
        yield return Addressables.InitializeAsync();
        yield return new WaitForSeconds(1);
        var asyncOperation = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        yield return asyncOperation;
        var scene = asyncOperation.Result;
        yield return new WaitForSeconds(3);
        yield return Addressables.UnloadSceneAsync(scene);
    }
}