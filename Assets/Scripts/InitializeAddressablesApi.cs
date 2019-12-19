using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitializeAddressablesApi : MonoBehaviour
{
    void Start()
    {
        Addressables.InitializeAsync();
    }
}
