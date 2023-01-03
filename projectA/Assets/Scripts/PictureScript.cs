using System.Collections;
using System.Collections.Generic;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PictureScript : MonoBehaviour
{
    [SerializeField] private GameObject _part1;
    [SerializeField] private GameObject _part2;
    [SerializeField] private GameObject _part3;

    //Player
    private HandStateManager _handStateManager;

    [SerializeField] private SavePlayerProgress _savePlayerProgress;
    [SerializeField] private GameObject _loadingScreen;


    public GameObject[] PictureParts;

    private GameObject _currentPart1;
    private GameObject _currentPart2;
    private GameObject _currentPart3;


    [SerializeField] private bool _isPicture1;
    [SerializeField] private bool _isPicture2;
    [SerializeField] private bool _isPicture3;

    [Space]

    [SerializeField] private Animator _diedTransitionAnimator;
    [SerializeField] private Volume _volume;

    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;

    [SerializeField] private bool _startLerpingEffects;
    [SerializeField] private bool _startScalingDistortion;

    private AudioSource _teleportSound;

    public void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        _volume.profile.TryGet(out _chromaticAberration);
        _volume.profile.TryGet(out _lensDistortion);

        _handStateManager = PlayerReference.HandStateManager;

        _teleportSound = this.GetComponent<AudioSource>();
    }

    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("MainPicture");

        writer.Write("Picture1", _isPicture1);
        writer.Write("Picture2", _isPicture2);
        writer.Write("Picture3", _isPicture3);

        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("MainPicture");

        _isPicture1 = reader.Read<bool>("Picture1");

        _isPicture2 = reader.Read<bool>("Picture2");

        _isPicture3 = reader.Read<bool>("Picture3");

        if (_isPicture1)
        {
            _part1.SetActive(true);
            Destroy(PictureParts[0]);
        }
        if (_isPicture2)
        {
            _part2.SetActive(true);
            Destroy(PictureParts[1]);
        }
        if (_isPicture3)
        {
            _part3.SetActive(true);
            Destroy(PictureParts[2]);
        }
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

    public void NextScene()
    {
        if(_isPicture1 && _isPicture2 && _isPicture3)
        {
            _teleportSound.Play();
            _startLerpingEffects = true;
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(2f);
        _startScalingDistortion = true;
        yield return new WaitForSeconds(0.3f);
        _diedTransitionAnimator.SetBool("isEnded", true);
        yield return new WaitForSeconds(2f);
        _savePlayerProgress.SaveCurrentScene();
        LoadScene(6);
        StopAllCoroutines();
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

    public void GetPicture()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "PicturePart1")
            {
                _currentPart1 = _handStateManager.CurrentItemHolder;
                _handStateManager.DropItem();

                _part1.SetActive(true);

                _isPicture1 = true;
                SaveCurrentProgress();
                NextScene();

                if (_handStateManager.CurrentItem != null)
                {
                    Destroy(_currentPart1);
                }
            }
        }

        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "PicturePart2")
            {
                _currentPart2 = _handStateManager.CurrentItemHolder;
                _handStateManager.DropItem();

                _part2.SetActive(true);

                _isPicture2 = true;
                SaveCurrentProgress();
                NextScene();

                if (_handStateManager.CurrentItem != null)
                {
                    Destroy(_currentPart2);
                }
            }
        }

        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "PicturePart3")
            {
                _currentPart3 = _handStateManager.CurrentItemHolder;
                _handStateManager.DropItem();

                _part3.SetActive(true);

                _isPicture3 = true;
                SaveCurrentProgress();
                NextScene();

                if (_handStateManager.CurrentItem != null)
                {
                    Destroy(_currentPart3);
                }
            }
        }
    }
}
