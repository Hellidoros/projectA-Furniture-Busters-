using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using CI.QuickSave;
using TMPro;
using UnityEngine.Events;
using System;

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

    [SerializeField] private TMP_FontAsset _globalFont;
    [SerializeField] private TMP_FontAsset _simpleFont;

    private List<TextMeshProUGUI> _allTexts = new List<TextMeshProUGUI>();

    public UnityEvent _changeLanguageEvents;

    public static event Action ChangeLanguageEvent;

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
        try
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

                if (_resoltuionDropdown != null)
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
        catch
        {
            Debug.Log("Root SettingMenu does not exist");
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
        if ( _uiManager != null &&_uiManager.PauseCanvas.activeSelf)
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
        _allTexts.Add(_languageText);
        _allTexts.Add(_graphicsText);
        _allTexts.Add(_musicText);
        _allTexts.Add(_soundText);
        _allTexts.Add(_sensivityText);
        _allTexts.Add(_fullscreenText);
        _allTexts.Add(_resolutionText);

        switch (LanguageInt)
        {
            case 0: // English
                _settingsText.text = "SETTINGS";
                _languageText.text = "LANGUAGE";
                _graphicsText.text = "GRAPHICS";
                _musicText.text = "MUSIC";
                _soundText.text = "SOUND";
                _sensivityText.text = "SENSIVITY";

                if (_fullscreenText != null)
                {
                    _fullscreenText.text = "FULL SCREEN";
                    _resolutionText.text = "RESOLUTION";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _simpleFont;
                }

                break;
            case 1: // Russian
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

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _simpleFont;
                }

                break;
            case 3: // Spanish
                _settingsText.text = "AJUSTES";
                _languageText.text = "IDIOMA";
                _graphicsText.text = "GRÁFICOS";
                _musicText.text = "MÚSICA";
                _soundText.text = "SONIDO";
                _sensivityText.text = "SENSIVIDAD";

                if (_fullscreenText != null)
                {
                    _fullscreenText.text = "PANTALLA COMPLETA";
                    _resolutionText.text = "RESOLUCIÓN";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 4: // German
                _settingsText.text = "EINSTELLUNGEN";
                _languageText.text = "SPRACHE";
                _graphicsText.text = "GRAFIK";
                _musicText.text = "MUSIK";
                _soundText.text = "TON"; _soundText.text = "TON";
                _sensivityText.text = "SENSIVITY";

                if (_fullscreenText != null)
                {
                    _fullscreenText.text = "VOLLBILDSCHIRM";
                    _resolutionText.text = "AUFLÖSUNG";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 5: // Japanese
                _settingsText.text = "設定";
                _languageText.text = "LANGUAGE";
                _graphicsText.text = "グラフィックス";
                _musicText.text = "音楽";
                _soundText.text = "サウンド";
                _sensivityText.text = "感度";

                if (_fullscreenText != null)
                {
                    _fullscreenText.text = "フルスクリーン";
                    _resolutionText.text = "解像度";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 6: // Portugese brazilian
                _settingsText.text = "SETTINGS";
                _languageText.text = "LANGUAGE";
                _graphicsText.text = "GRÁFICOS";
                _musicText.text = "MÚSICA";
                _soundText.text = "SOUND";
                _sensivityText.text = "SENSIVIDADE";

                if (_fullscreenText.text != null)
                {
                    _fullscreenText.text = "FULL SCREEN";
                    _resolutionText.text = "RESOLUÇÃO";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 7: // Korean
                _settingsText.text = "설정";
                _languageText.text = "언어";
                _graphicsText.text = "그래픽";
                _musicText.text = "음악";
                _soundText.text = "소리";
                _sensivityText.text = "민감도";

                if (_fullscreenText != null)
                {
                    _fullscreenText.text = "전체 화면";
                    _resolutionText.text = "해상도";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 8: // Indonesian
                _settingsText.text = "PENGATURAN";
                _languageText.text = "BAHASA";
                _graphicsText.text = "GRAFIK";
                _musicText.text = "MUSIK";
                _soundText.text = "SUARA";
                _sensivityText.text = "SENSIVITAS";

                if (_fullscreenText != null)
                {
                    _fullscreenText.text = "LAYAR PENUH";
                    _resolutionText.text = "RESOLUSI";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 9: //Chinese
                _settingsText.text = "设置";
                _languageText.text = "语言";
                _graphicsText.text = "图形";
                _musicText.text = "音乐";
                _soundText.text = "声音";
                _sensivityText.text = "灵敏度";

                if(_fullscreenText != null)
                 {
                    _fullscreenText.text = "全屏";
                    _resolutionText.text = "决议";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
            case 10: // Thai
                _settingsText.text = "การตั้งค่า";
                _languageText.text = "ภาษา";
                _graphicsText.text = "กราฟิก";
                _musicText.text = "เพลง";
                _soundText.text = "เสียง";
                _sensivityText.text = "ความละเอียดอ่อน";

                if(_fullscreenText != null)
                {
                    _fullscreenText.text = "เต็มหน้าจอ";
                    _resolutionText.text = "แก้ไข";
                }

                foreach (TextMeshProUGUI text in _allTexts)
                {
                    text.font = _globalFont;
                }

                break;
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
        ChangeLanguageEvent?.Invoke();
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
