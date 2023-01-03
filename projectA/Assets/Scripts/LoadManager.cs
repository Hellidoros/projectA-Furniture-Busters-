using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public void  Start()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (operation.progress > 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        operation.allowSceneActivation = true;
    }
}
