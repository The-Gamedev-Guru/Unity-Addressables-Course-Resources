using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TheGamedevGuru
{
/// <summary>
/// Container of network pools categorized by type.
/// </summary>
public static class NetworkPools
{
  private const int InitialElementCount = 0;

  private static readonly Dictionary<AssetReference, NetworkPool> Pools =  new Dictionary<AssetReference, NetworkPool>();

  public static NetworkPool GetPool(AssetReference prefabReference)
  {
    if (Pools.ContainsKey(prefabReference))
    {
      return Pools[prefabReference];
    }

    var networkPool = (new GameObject()).AddComponent<NetworkPool>();
    networkPool.Setup(InitialElementCount, prefabReference);
    Object.DontDestroyOnLoad(networkPool.gameObject);
    return Pools[prefabReference] = networkPool;
  }

  /// <summary>
  /// Clear and empty all the pools to release memory.
  /// You can call this on level changes, based on timers, etc..
  /// </summary>
  public static void EmptyPools()
  {
    foreach (var poolKeyValuePair in Pools)
    {
      Object.Destroy(poolKeyValuePair.Value);
    }

    Pools.Clear();
  }
}
}