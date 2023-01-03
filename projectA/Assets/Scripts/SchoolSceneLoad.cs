using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SchoolSceneLoad : MonoBehaviour
{
    [SerializeField] private Animator _diedTransitionAnimator;
    [SerializeField] private SavePlayerProgress _savePlayerProgress;
    [SerializeField] private GameObject _loadingScreen;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _diedTransitionAnimator.SetBool("isEnded", true);
            _savePlayerProgress.SaveCurrentScene();
            StartCoroutine(Timer());
        }
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(2);
        LoadScene(4);
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        _loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (operation.progress > 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

    }

    private void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }
}
