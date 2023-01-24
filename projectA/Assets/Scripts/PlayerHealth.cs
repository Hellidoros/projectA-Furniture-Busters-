using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using CI.QuickSave;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private float _alertVignette = 0.5f;
    [SerializeField] private float _normalFog;
    [SerializeField] private float _chaseFog;
    [SerializeField] private float _changeSpeed = 0.5f;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private Animator _diedTransitionAnimator;
    [SerializeField] private Animator _heartsAnimator;
    [SerializeField] private TextMeshProUGUI _heartsAmountText;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private SavePlayerProgress _savePlayerProgress;

    [SerializeField] private Image _fixedJoystickImage;
    [SerializeField] private Image _fixedJoystickImageHandle;
    public GameObject[] _playerCanvas;
    public GameObject[] PlaceToHide;

    private float _normalVignette;

    private Vignette _vignette;
    private float _currentVignetteValue;

    public static int HeartsAmount = 5;

    public bool Alive = true;
    public bool Chase = false;
    public bool IsHiding;

    //Getters setters
    public Vignette Vignette { get { return _vignette; } }
    public float AlertVignette {get { return _alertVignette; } }
    public float NormalVignette { get { return _normalVignette; } }

    private void Start()
    {
        //SavePlayerProgress.SaveEvent += SaveCurrentProgress;

        GetSavedReferences();

        _volume.profile.TryGet(out _vignette);
        _normalVignette = _vignette.smoothness.value;
        IsHiding = false;

        if(HeartsAmount <= 0)
        {
            if (UIManager.SmartphoneInput)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            _endGamePanel.SetActive(true);
        }

        //GetSavedReferences();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsHiding)
        {
            if (other.gameObject.name == "eyes")
            {
                other.transform.parent.GetComponent<Monster>().CheckSight();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!IsHiding)
        {
            if (other.gameObject.name == "eyes")
            {
                other.transform.parent.GetComponent<Monster>().CheckSight();
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name == "eyes" && Alive == true && !Chase)
    //    {
    //        _musicManager.StartAtmosphereSound();
    //        _vignette.smoothness.value = _normalVignette;
    //    }
    //}

    public void StartChaseEffect()
    {
        if (!Chase)
        {
            Chase = true;
            _musicManager.StartMonsterNearSound();
            _vignette.smoothness.value = _alertVignette;
            RenderSettings.fogDensity = _chaseFog;

            foreach(GameObject gameObject in PlaceToHide)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public void StopChasseEffect()
    {
        Chase = false;
        _musicManager.StartAtmosphereSound();

        foreach (GameObject gameObject in PlaceToHide)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (!IsHiding)
        {
            _vignette.smoothness.value = _normalVignette;
            RenderSettings.fogDensity = _normalFog;
        }
    }

    public void StartHideEffect()
    {
        _vignette.smoothness.value = 0.8f;
        RenderSettings.fogDensity = _chaseFog;
    }

    public void StopHideEffect()
    {
        _vignette.smoothness.value = _normalVignette;
        RenderSettings.fogDensity = _normalFog;
    }


    private IEnumerator Timer()
    {
        _diedTransitionAnimator.SetBool("isEnded", true);
        yield return new WaitForSeconds(2f);
        _heartsAnimator.SetTrigger("Start");

        var heartsAmountbeforeDeath = HeartsAmount + 1;

        _heartsAmountText.text = heartsAmountbeforeDeath.ToString();
        _heartsAmountText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        _heartsAnimator.SetTrigger("Kill");
        _heartsAmountText.text = HeartsAmount.ToString();

        yield return new WaitForSeconds(3f);

        if(HeartsAmount != 0)
        {
            if (_savePlayerProgress != null)
            {
                _savePlayerProgress.SaveCurrentScene();
            }

            if (UIManager.SmartphoneInput)
            {
                //AppodealManager.ShowInterAds();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
        else if(HeartsAmount <= 0)
        {
            if (UIManager.SmartphoneInput)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            _endGamePanel.SetActive(true);
        }
    }

    private IEnumerator EnableFPController()
    {
        yield return new WaitForSeconds(1f);
        this.GetComponent<FirstPersonController>().enabled = true;
        StopAllCoroutines();
    }

   public void KillPLayer()
    {
        if (_savePlayerProgress != null)
        {
            _savePlayerProgress.SaveCurrentScene();
        }

        if(HeartsAmount != 0)
        {
            HeartsAmount -= 1;
            SaveCurrentProgress();
        }
        else
        {
            MenuManager.LoadProgress = false;
        }


        StartCoroutine(Timer());
    }

    public void DisableCanvas()
    {
        if (UIManager.SmartphoneInput)
        {
            foreach (GameObject gameObject in _playerCanvas)
            {
                gameObject.SetActive(false);
            }

            if (_fixedJoystickImage != null)
            {
                _fixedJoystickImage.enabled = false;
                _fixedJoystickImageHandle.enabled = false;
            }
        }
    }

    public void EnableCanvas()
    {
        if (UIManager.SmartphoneInput)
        {
            foreach (GameObject gameObject in _playerCanvas)
            {
                gameObject.SetActive(true);
            }

            if (_fixedJoystickImage != null)
            {
                _fixedJoystickImage.enabled = true;
                _fixedJoystickImageHandle.enabled = true;
            }
        }
    }

    public static void SetHeartsAmount(int amount)
    {
        HeartsAmount = amount;
        SaveCurrentProgress();
    }

    public static void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("PlayerHealth");

        writer.Write("HeartsAmount", HeartsAmount);

        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("PlayerHealth");

        HeartsAmount = reader.Read<int>("HeartsAmount");
    }
}
