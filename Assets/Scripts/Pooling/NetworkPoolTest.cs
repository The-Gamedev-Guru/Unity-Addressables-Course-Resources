using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheGamedevGuru
{
public class NetworkPoolTest : MonoBehaviour
{
    [SerializeField] private int instancesToSpawn = 5;
    [SerializeField] private AssetReference soldierReference = null;
    [SerializeField] private AssetReference tankReference = null;
    
    IEnumerator Start()
    {
        // Wait for pool to warm up for our test
        var soldierPool = NetworkPools.GetPool(soldierReference);
        var tankPool = NetworkPools.GetPool(tankReference);
        yield return new WaitForSeconds(3);
        
        // Take instances of each prefab and parent them here.
        // Some of them won't be pooled yet (adjust the initial capacity on the automatic pool to avoid this).
        var soldierInstances = new Stack<GameObject>();
        var tankInstances = new Stack<GameObject>();
        for (var i = 0; i < instancesToSpawn; i++)
        {
            var newSoldier = soldierPool.Take(transform);
            var newTank = tankPool.Take(transform);
            newSoldier.transform.localPosition = Vector3.right * i;
            newTank.transform.localPosition = Vector3.right * i + Vector3.forward*2;
            soldierInstances.Push(newSoldier);
            tankInstances.Push(newTank);
        }
        
        // Return some of them
        yield return new WaitForSeconds(2);
        while (soldierInstances.Count > 2)
        {
            soldierPool.Return(soldierInstances.Pop());
        }
        while (tankInstances.Count > 1)
        {
            tankPool.Return(tankInstances.Pop());
        }
        
        // Destroy this script to clean up the whole thing.
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed. Disposing pool");
        NetworkPools.EmptyPools();
    }
}
}
