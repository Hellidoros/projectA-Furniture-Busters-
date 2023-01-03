using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class FirstSceneManager : MonoBehaviour
{
    [SerializeField] private Animator _diedTransitionAnimator;
    [SerializeField] private Volume _volume;

    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;

    [SerializeField] private bool _startLerpingEffects;
    [SerializeField] private bool _startScalingDistortion;

    [SerializeField] private AudioSource _teleportAudio;
    [SerializeField] private GameObject _monster;

    public void Start()
    {
        _volume.profile.TryGet(out _chromaticAberration);
        _volume.profile.TryGet(out _lensDistortion);
    }

    private IEnumerator Timer()
    {
        _teleportAudio.Play();
        _monster.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        _monster.SetActive(false);
        yield return new WaitForSeconds(1.3f);
        _startScalingDistortion = true;
        yield return new WaitForSeconds(0.4f);
        _diedTransitionAnimator.SetBool("isEnded", true);
        yield return new WaitForSeconds(2f);
        LoadScene(3);
        StopAllCoroutines();
    }

    public void Update()
    {
        if (_startLerpingEffects)
        {
            _chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, 1f, 0.02f);

            _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, -0.7f, 0.03f);
        }

        if (_startScalingDistortion)
        {
            _lensDistortion.scale.value = Mathf.Lerp(_lensDistortion.scale.value, 0f, 0.07f);
        }
    }

    public IEnumerator LoadSceneAsync(int sceneId)
    {
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


    public void NextScene()
    {
        _startLerpingEffects = true;
        StartCoroutine(Timer());
    }
}
