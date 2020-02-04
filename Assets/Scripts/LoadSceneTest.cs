using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadSceneTest : MonoBehaviour      //얘가 서브씬을 로드하고, 언로드 해야 함
{
    private AsyncOperationHandle<GameObject> handle;
    private AsyncOperationHandle bundleHandle;

    public string lable;
    public Button spawnButton;
    public Text loadingText;

    public void BundleDown()
    {        
        StartCoroutine(BundleDownRoutine());
    }

    private void LoadSceneTest_Completed1(AsyncOperationHandle obj)
    {
        bundleHandle = obj;
        Debug.Log("로드 완료!");
        loadingText.text = "Done Loading";
        spawnButton.GetComponent<Image>().color = Color.red;
    }

    private void LoadSceneTest_Completed(AsyncOperationHandle<long> obj)
    {
        Debug.Log(obj.Result);
    }

    public void Spawn()
    {
        CreateCube();
    }

    private IEnumerator BundleDownRoutine()
    {        
        bundleHandle = Addressables.DownloadDependenciesAsync(lable);

        while(true)
        {
            if(!handle.IsDone)
            {
                Debug.Log(handle.PercentComplete);
                loadingText.text = handle.PercentComplete + "%";
                yield return null;
            }

            Debug.Log("로드 완료!");
            loadingText.text = "Done Loading";
            spawnButton.GetComponent<Image>().color = Color.red;
            break;
        }
    }

    private void CreateCube()
    {
        Addressables.LoadAssetAsync<GameObject>("Cube").Completed += InstantiateCube_Completed;
    }

    private void InstantiateCube_Completed(AsyncOperationHandle<GameObject> obj)
    {
        handle = obj;
        GameObject cube = handle.Result;
        Instantiate(cube, Vector3.zero, Quaternion.identity);
    }

    private void ReleaseAsset()
    {
        Addressables.Release(handle);
    }
}
