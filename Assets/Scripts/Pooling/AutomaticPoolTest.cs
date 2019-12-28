using System.Collections;
using UnityEngine;

namespace TheGamedevGuru
{
public class AutomaticPoolTest : MonoBehaviour
{
  [SerializeField] private int instancesToSpawn = 10;
  [SerializeField] private AutomaticPool automaticPool = null;

  IEnumerator Start()
  {
    // Wait for pool to warm up for our test
    yield return new WaitForSeconds(3);

    // Take about 10 tanks and parent them here.
    // Some of them won't be pooled yet (adjust the initial capacity on the automatic pool to avoid this).
    for (var i = 0; i < instancesToSpawn; i++)
    {
      var newInstance = automaticPool.Take(transform);
      newInstance.transform.localPosition = Vector3.right * i;
    }

    // Wait
    yield return new WaitForSeconds(2);

    // Return all elements
    while (transform.childCount > 0)
    {
      automaticPool.Return(transform.GetChild(0).gameObject);
    }
  }
}
}