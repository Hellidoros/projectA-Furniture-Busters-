using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class ForestSceneManager : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private Animator _transitionAnimator;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _loadingScreen;

    [SerializeField] private GameObject _playerCam;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private GameObject _virtualCam;


    public GameObject PlayerItemsRendererCamera;

    public static bool IsSceneFinished;

    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        if (MenuManager.LoadProgress)
        {
            GetSavedReferences();
        }

        IsSceneFinished = false;
        SaveCurrentProgress();
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("ForestScene");
        writer.Write("IsFinished", IsSceneFinished);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("ForestScene");

        if (reader.Exists("IsFinished"))
        {
            IsSceneFinished = reader.Read<bool>("IsFinished");
        }
    }

    private void OnDisable()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void NextScene()
    {
        IsSceneFinished = true;
        SaveCurrentProgress();

        StartCoroutine(Timer());
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(2f);
        _playerHealth.DisableCanvas();

        _playerCam.SetActive(false);
        _virtualCam.SetActive(true);

        PlayerItemsRendererCamera.transform.parent = _virtualCam.transform;
        PlayerItemsRendererCamera.transform.position = _virtualCam.transform.position;
        PlayerItemsRendererCamera.transform.rotation = _virtualCam.transform.rotation;

        _handAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        _transitionAnimator.SetBool("isEnded", true);
        yield return new WaitForSeconds(2f);
        LoadScene(3);
    }

    public IEnumerator LoadSceneAsync(int sceneId)
    {
        _loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }
}
