using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TheGamedevGuru
{
public class ParticleSystemLoader : MonoBehaviour
{
    [SerializeField] private AssetReference explosionPrefabReference = null;

    public void SpawnExplosion(Vector3 position)
    {
        StartCoroutine(ManageParticleSystem(explosionPrefabReference, position));
    }

    private IEnumerator ManageParticleSystem(AssetReference prefabReference, Vector3 position)
    {
        var asyncOperation = explosionPrefabReference.InstantiateAsync(position, Quaternion.identity, transform);
        yield return asyncOperation;
        var prefab = asyncOperation.Result;
        var particleSystems = prefab.GetComponentsInChildren<ParticleSystem>();
        Assert.IsTrue(particleSystems.Length > 0);
        //yield return new WaitForSeconds(5);
        foreach (var particleSystem in particleSystems)
        {
            Assert.IsFalse(particleSystem.main.loop);
            Assert.IsTrue(Mathf.Approximately(particleSystem.emission.rateOverTime.constant, 0f));
            Assert.IsTrue(Mathf.Approximately(particleSystem.emission.rateOverDistance.constant, 0f));
            yield return new WaitUntil(() => particleSystem.isEmitting == false);
        }
        Addressables.ReleaseInstance(asyncOperation);
    }

    IEnumerator Start()
    {
        for (var i = 0; i < 5; i++)
        {
            SpawnExplosion(Vector3.right * i);
            yield return new WaitForSeconds(.2f);
        }
    }
}
}