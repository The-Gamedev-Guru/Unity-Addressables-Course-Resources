using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheGamedevGuru
{
public class AddressablesJsonLoader : MonoBehaviour
{
    [SerializeField] private AssetReference jsonReference = null;
    
    IEnumerator Start()
    {
        var asyncOperation = jsonReference.LoadAssetAsync<TextAsset>();
        yield return asyncOperation;
        var jsonText = asyncOperation.Result.text;
        Debug.Log("JSON content: " + jsonText);
        //var jsonData = JsonUtility.FromJson<YourCustomJsonDataStructure>(jsonText);
    }
}
}
