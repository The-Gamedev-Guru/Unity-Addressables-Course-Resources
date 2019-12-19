using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace TheGamedevGuru
{
public class MusicPlayer : MonoBehaviour
{
    //[SerializeField] private AudioClip[] soundTrackDirectReferences = null;
    [SerializeField] private AssetReference[] soundTrackIndirectReferences = null;
    
    private AudioSource _audioSource = null;
    
    void PlaySoundtrack(int index)
    {
        StartCoroutine(PlaySoundTrackIndirect(index));
    }

    private IEnumerator PlaySoundTrackIndirect(int index)
    {
        yield return new WaitForSeconds(5);
        var asyncOperationHandle = soundTrackIndirectReferences[index].LoadAssetAsync<AudioClip>();
        //var asyncOperationHandle = Addressables.LoadAssetAsync<AudioClip>("Bobs-Theme");
        //var asyncOperationHandle = Addressables.LoadAssetsAsync<AudioClip>("Gameplay", null);
        yield return asyncOperationHandle;
        _audioSource.clip = asyncOperationHandle.Result;
        _audioSource.Play();
        yield return new WaitUntil(() => _audioSource.isPlaying == false);
        _audioSource.clip = null;
        Addressables.Release(asyncOperationHandle);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlaySoundtrack(1);
    }
}
}
