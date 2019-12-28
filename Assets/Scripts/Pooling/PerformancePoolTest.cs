using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheGamedevGuru
{
public class PerformancePoolTest : MonoBehaviour
{
  [SerializeField] private int instancesToPoolAndSpawn = 10;
  [SerializeField] private AssetReference prefabReference = null;
  private readonly PerformancePool<SoldierBehavior> _soldierPerformancePool = new PerformancePool<SoldierBehavior>();

  IEnumerator Start()
  {
    // Wait for pool to warm up for our test
    _soldierPerformancePool.Setup(prefabReference, instancesToPoolAndSpawn);
    yield return new WaitForSeconds(3);

    // Take about 10 tanks and parent them here.
    // Some of them won't be pooled yet (adjust the initial capacity on the automatic pool to avoid this).
    var soldiers = new List<SoldierBehavior>();
    for (var i = 0; i < instancesToPoolAndSpawn; i++)
    {
      var newInstance = _soldierPerformancePool.Take();
      var soldierTransform = newInstance.transform;
      soldierTransform.position = transform.position + Vector3.right * i;
      soldiers.Add(newInstance);
    }

    // Wait
    yield return new WaitForSeconds(2);

    // Return some elements
    while (soldiers.Count > 2)
    {
      _soldierPerformancePool.Return(soldiers[0]);
      soldiers.RemoveAt(0);
    }
  }

  private void OnDestroy()
  {
    Debug.Log("Destroyed. Disposing pool");
    _soldierPerformancePool.Dispose();
  }
}
}