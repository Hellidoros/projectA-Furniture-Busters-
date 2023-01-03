using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static bool SmartphoneInput;

    public bool EnableSmartPhoneInput;

    public GameObject PauseCanvas;
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _player;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private SavePlayerProgress _savePlayerProgress;
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _diedCanvas;

    [SerializeField] private GameObject _globalVolume;

    [Space]

    [SerializeField] private TextMeshProUGUI _continueText;
    [SerializeField] private TextMeshProUGUI _optionsText;
    [SerializeField] private TextMeshProUGUI _mainMenuText;

    public bool IsPaused = false;
    private bool _isFogEnabled = true;
    private bool _isVolumeEnabled = true;
    private bool _isSettingsEnabled;

    public void Start()
    {
        SmartphoneInput = EnableSmartPhoneInput;
        ChangeLanguage();

        if (!SmartphoneInput)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || CrossPlatformInputManager.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape) && !IsPaused)
        {
            SetActivePause(true);
        }
        else if (Input.GetKeyDown(KeyCode.P) || CrossPlatformInputManager.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape) && IsPaused)
        {
            SetActivePause(false);
        }
    }

    public void ReturnMainMenu()
    {
        if (_savePlayerProgress)
        {
            _savePlayerProgress.SaveCurrentScene();
        }
        LoadScene(1);
        Time.timeScale = 1;
    }

    private void OnApplicationPause(bool pause)
    {
        SetActivePause(true);
        _diedCanvas.SetActive(false);
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

    public void ChangeLanguage()
    {
        if(SettingMenu.LanguageInt == 0)
        {
            _continueText.text = "CONTINUE";
            _optionsText.text = "OPTIONS";
            _mainMenuText.text = "MAIN MENU";
        }
        else if(SettingMenu.LanguageInt == 1)
        {
            _continueText.text = "ПРОДОЛЖИТЬ";
            _optionsText.text = "НАСТРОЙКИ";
            _mainMenuText.text = "ГЛАВНОЕ МЕНЮ";
        }
    }

    public void ManageShadow()
    {
        _isFogEnabled = !_isFogEnabled;
        RenderSettings.fog = _isFogEnabled;
    }

    public void ManageVolume()
    {
        _isVolumeEnabled = !_isVolumeEnabled;
        _globalVolume.SetActive(_isVolumeEnabled);
    }

    public void SetActiveSettings()
    {
        _isSettingsEnabled = !_isSettingsEnabled;
        _settingsCanvas.SetActive(_isSettingsEnabled);
    }


    public void SetActivePause(bool state)
    {
        IsPaused = state;

        PauseCanvas.SetActive(state);
        //_player.SetActive(!state);
        _firstPersonController.enabled = !state;

        _diedCanvas.SetActive(!state);

        if (!EnableSmartPhoneInput)
        {
            if (state)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = state;
        }

        Time.timeScale = state ? 0 : 1;
    }
}
