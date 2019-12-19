using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

namespace TheGamedevGuru
{
[RequireComponent(typeof(VideoPlayer))]
public class AddressablesVideoPlayer : MonoBehaviour
{
    [SerializeField] private AssetReference[] videoClipRefs = null;
    private VideoPlayer videoPlayer;
    
    public void PlayVideo(int index)
    {
        StartCoroutine(PlayVideoInternal(index));
    }

    private IEnumerator PlayVideoInternal(int index)
    {
        var asyncOperationHandle = videoClipRefs[index].LoadAssetAsync<VideoClip>();
        yield return asyncOperationHandle;
        videoPlayer.clip = asyncOperationHandle.Result;
        videoPlayer.Play();
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => videoPlayer.isPlaying == false);
        videoPlayer.clip = null;
        Addressables.Release(asyncOperationHandle);
    }

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        PlayVideo(1);
    }
}
}