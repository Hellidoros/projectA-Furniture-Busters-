using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using CI.QuickSave;
using TMPro;
using UnityEngine.Events;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer AudioMusicMixer;
    public AudioMixer AudioSoundsMixer;
    public FirstPersonController FirstPersonController;

    [SerializeField] private GameObject _mobileCanvas;
    [SerializeField] private GameObject _deskCanvas;

    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private UIManager _uiManager;


    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private TMP_Dropdown _languageDropdown;
    [SerializeField] private TMP_Dropdown _resoltuionDropdown;

    [SerializeField] private float _soundVolume;
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _mouseSensivity;
    [SerializeField] private int _qualityIndex = 1;

    [Space]

    [SerializeField] private Slider _musicSLider;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _sensivitySlider;

    [Space]
    [SerializeField] private TextMeshProUGUI _settingsText;

    [SerializeField] private TextMeshProUGUI _languageText;
    [SerializeField] private TextMeshProUGUI _graphicsText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private TextMeshProUGUI _soundText;
    [SerializeField] private TextMeshProUGUI _sensivityText;
    [SerializeField] private TextMeshProUGUI _fullscreenText;
    [SerializeField] private TextMeshProUGUI _resolutionText;

    public UnityEvent _changeLanguageEvents;

    private bool _languageHasChanged = false;
    private bool _resolutionChanged = false;
    private bool _isFullscreen = true;
    private int _savedResolutionIndex;
    public static int LanguageInt;

    private Resolution[] _resolutions;

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("SettingManager" + this.gameObject.name);

        writer.Write("SoundVolume", _soundVolume);
        writer.Write("MusicVolume", _musicVolume);
        writer.Write("MouseSensivity", _mouseSensivity);
        writer.Write("Qualitty", _qualityIndex);
        writer.Write("LanguageChanged", _languageHasChanged);
        writer.Write("Language", LanguageInt);
        writer.Write("Fullscreen", _isFullscreen);
        writer.Write("ResolutionIndex", _savedResolutionIndex);
        writer.Write("ResolutionChanged", _resolutionChanged);

        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("SettingManager" + this.gameObject.name);

        LanguageInt = reader.Read<int>("Language");
        _languageHasChanged = reader.Read<bool>("LanguageChanged");

        _resolutionChanged = reader.Read<bool>("ResolutionChanged");
        _soundVolume = reader.Read<float>("SoundVolume");
        _musicVolume = reader.Read<float>("MusicVolume");
        _mouseSensivity = reader.Read<float>("MouseSensivity");
        _qualityIndex = reader.Read<int>("Qualitty");

        SetMusicVolume(_musicVolume);
        SetSoundsVolume(_soundVolume);
        SetMouseSensivity(_mouseSensivity);
        SetQuality(_qualityIndex);

        if (!UIManager.SmartphoneInput)
        {
            _isFullscreen = reader.Read<bool>("Fullscreen");
            _savedResolutionIndex = reader.Read<int>("ResolutionIndex");

            SetFullScreen(_isFullscreen);

            if(_resoltuionDropdown != null)
            {
                if (_resolutionChanged)
                {
                    SetResolution(_savedResolutionIndex);
                }
            }
        }

        _qualityDropdown.value = _qualityIndex;
        _languageDropdown.value = LanguageInt;

        _languageDropdown.value = LanguageInt;
        _musicSLider.value = _musicVolume;
        _volumeSlider.value = _soundVolume;
        _sensivitySlider.value = _mouseSensivity;


        if (_languageHasChanged)
        {
            ChangeLanguage();
        }
        else
        {
            InitializeLanguage();
        }

    }

    public void ManageSettings()
    {
        if (UIManager.SmartphoneInput)
        {
            _mobileCanvas.SetActive(true);
        }
        else
        {
            _deskCanvas.SetActive(true);
        }
    }

    private void Start()
    {
        if (UIManager.SmartphoneInput)
        {
            Application.runInBackground = true;
            //Set fixed framerate
            Application.targetFrameRate = 30;
        }

        if (!UIManager.SmartphoneInput)
        {
            GetResolutions();
        }

        GetSavedReferences();
    }

    private void OnEnable()
    {
        GetSavedReferences();
    }

    private void Update()
    {
        if ( _uiManager != null &&_uiManager.PauseCanvas.activeSelf )
        {
            if (!UIManager.SmartphoneInput)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (_uiManager.PauseCanvas.activeSelf)
                    {
                        _deskCanvas.SetActive(false);
                        _uiManager.IsPaused = false;
                        _uiManager.SetActivePause(true);
                    }
                }
            }
        }
    }

    public void GetResolutions()
    {
        if(_resoltuionDropdown != null)
        {
            _resolutions = Screen.resolutions;
            _resoltuionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            _resoltuionDropdown.AddOptions(options);
            _resoltuionDropdown.value = currentResolutionIndex;
            _resoltuionDropdown.RefreshShownValue();
        }
    }

    public void ChangeLanguage()
    {
        if (LanguageInt == 0)
        {
            _settingsText.text = "SETTINGS";
            _languageText.text = "LANGUAGE";
            _graphicsText.text = "GRAPHICS";
            _musicText.text = "MUSIC";
            _soundText.text = "SOUND";
            _sensivityText.text = "SENSIVITY";

            if(_fullscreenText != null)
            {
                _fullscreenText.text = "FULL SCREEN";
                _resolutionText.text = "RESOLUTION";
            }
        }
        else if (LanguageInt == 1)
        {
            _settingsText.text = "НАСТРОЙКИ";
            _languageText.text = "ЯЗЫК";
            _graphicsText.text = "ГРАФИКА";
            _musicText.text = "МУЗЫКА";
            _soundText.text = "ЗВУК";
            _sensivityText.text = "ЧУВСТВИТЕЛЬНОСТЬ";

            if (_fullscreenText != null)
            {
                _fullscreenText.text = "ПОЛНЫЙ ЭКРАН";
                _resolutionText.text = "РАЗРЕШЕНИЕ";
            }
        }
    }

    private void InitializeLanguage()
    {
        if (!_languageHasChanged)
        {
            _languageHasChanged = true;
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                _languageDropdown.value = _languageDropdown.options.FindIndex(option => option.text == "Русский");
                LanguageInt = 1;
            }
            if (Application.systemLanguage == SystemLanguage.English)
            {
                _languageDropdown.value = _languageDropdown.options.FindIndex(option => option.text == "English");
                LanguageInt = 0;
            }

            SetLanguage(LanguageInt);
        }
    }

    public void SetLanguage(int language)
    {
        LanguageInt = language;
        _languageHasChanged = true;

        if(_menuManager != null)
        {
            _menuManager.ChangeLanguageOfText();
        }
        if(_uiManager != null)
        {
            _uiManager.ChangeLanguage();
        }

        if(_changeLanguageEvents != null)
        {
            _changeLanguageEvents.Invoke();
        }
        ChangeLanguage();
        SaveCurrentProgress();
    }

    public void SetMusicVolume(float volume)
    {
        AudioMusicMixer.SetFloat("Volume", volume);
        _musicVolume = volume;

        SaveCurrentProgress();
    }

    public void SetSoundsVolume(float volume)
    {
        AudioSoundsMixer.SetFloat("Volume", volume);
        _soundVolume = volume;

        SaveCurrentProgress();
    }

    public void SetMouseSensivity(float sensivity)
    {
        if(FirstPersonController != null)
        {
            FirstPersonController.MouseXSensivity = sensivity;
            FirstPersonController.MouseYSensivity = sensivity;
        }

        _mouseSensivity = sensivity;

        SaveCurrentProgress();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        _qualityIndex = qualityIndex;
        _qualityDropdown.value = _qualityIndex;

        SaveCurrentProgress();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullscreen = isFullScreen;
        Screen.fullScreen = isFullScreen;

        SaveCurrentProgress();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        _savedResolutionIndex = resolutionIndex;
        _resolutionChanged = true;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        SaveCurrentProgress();
    }
}
