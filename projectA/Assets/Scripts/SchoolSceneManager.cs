using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CI.QuickSave;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SchoolSceneManager : MonoBehaviour
{
    public Animator TransitionAnimator;
    public static bool IsSceneFinished;
    [SerializeField] private GameObject _loadingScreen;

    [SerializeField] private Volume _volume;

    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;

    [SerializeField] private bool _startLerpingEffects;
    [SerializeField] private bool _startScalingDistortion;


    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        if (MenuManager.LoadProgress)
        {
            GetSavedReferences();
        }

        IsSceneFinished = false;
        SaveCurrentProgress();


        _volume.profile.TryGet(out _chromaticAberration);
        _volume.profile.TryGet(out _lensDistortion);
    }

    private void Update()
    {
        if (_startLerpingEffects)
        {
            _chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, 1f, 0.02f);

            _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, -0.7f, 0.01f);
        }

        if (_startScalingDistortion)
        {
            _lensDistortion.scale.value = Mathf.Lerp(_lensDistortion.scale.value, 0f, 0.07f);
        }
    }

    private void OnDisable()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("SchooltScene");
        writer.Write("IsFinished", IsSceneFinished);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("SchooltScene");

        IsSceneFinished = reader.Read<bool>("IsFinished");
    }


    public void NextScene()
    {
        _startLerpingEffects = true;
        IsSceneFinished = true;
        SaveCurrentProgress();
        StartCoroutine(Timer());
    }

    public IEnumerator Timer()
    {
        //yield return new WaitForSeconds(3f);
        //TransitionAnimator.SetBool("isEnded", true);
        //yield return new WaitForSeconds(1f);
        //SceneManager.LoadScene(2, LoadSceneMode.Single);

        yield return new WaitForSeconds(1f);
        _startLerpingEffects = true;
        yield return new WaitForSeconds(2f);
        _startScalingDistortion = true;
        yield return new WaitForSeconds(0.3f);
        TransitionAnimator.SetBool("isEnded", true);
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
