using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DlcManager : MonoBehaviour
{
    private const string DlcName = "DLC-Tough-Times";
    private bool _hasBoughtDlc = true;    // API-dependant.
    [SerializeField] private Button playDlcButton = null;
    
    public void OnPlay()
    {
        SceneManager.LoadScene("Level-1");
    }
    public void OnPlayDlc()
    {
        if (_hasBoughtDlc)
        {
            playDlcButton.enabled = false;
            StartCoroutine(OnPlayDlcInternal());
        }
    }

    private IEnumerator OnPlayDlcInternal()
    {
        // 1. Download DLC if needed.
        var downloadSizeHandle = Addressables.GetDownloadSizeAsync(DlcName);
        yield return downloadSizeHandle;
        var downloadSizeBytes = downloadSizeHandle.Result;
        var dlcAlreadyDownloaded = downloadSizeBytes == 0;
        
        if (dlcAlreadyDownloaded)
        {
            Debug.Log("DLC already downloaded");
        }
        else
        {
            var totalMb = downloadSizeBytes / 1024 / 1024;
            var downloadDependenciesHandle = Addressables.DownloadDependenciesAsync(DlcName);
            while (downloadDependenciesHandle.IsDone == false)
            {
                yield return null;
                if (downloadDependenciesHandle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError("That didn't work");
                    yield break;
                }
                var currentMb = downloadDependenciesHandle.PercentComplete * downloadSizeBytes / 1024 / 1024;
                // mySliderProgressbar.value = downloadDependenciesHandle.PercentComplete;
                Debug.Log($"{downloadDependenciesHandle.PercentComplete * 100f:0}% ({currentMb:0}/{totalMb:0} MB)");
            }
            Debug.Log("Download complete");
        }
        
        // 2. Load
        playDlcButton.enabled = true;    // Reset safe-guard
        var loadSceneHandle = Addressables.LoadSceneAsync("DLC-Level-1");
        yield return loadSceneHandle;
    }
}
