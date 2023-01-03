using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CI.QuickSave;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _continueGame;
    public static bool LoadProgress = false;
    [SerializeField] private bool EnableContinue;
    [SerializeField] private GameObject _loadingScreen;


    [SerializeField] private TextMeshProUGUI _continueText;
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private TextMeshProUGUI _optionText;
    [SerializeField] private TextMeshProUGUI _exitText;

    [Space]

    [SerializeField] private TextMeshProUGUI _difficultyText;
    [SerializeField] private TextMeshProUGUI _easyText;
    [SerializeField] private TextMeshProUGUI _normalText;
    [SerializeField] private TextMeshProUGUI _hardText;
    [SerializeField] private TextMeshProUGUI _playText;

    private void Awake()
    {
        GetSavedReferences();
    }

    private void Start()
    {
        Debug.Log(QuickSaveGlobalSettings.StorageLocation);
        StartCoroutine(Timer());
        GetSavedReferences();
    }

    public void ChangeLanguageOfText()
    {
        if(SettingMenu.LanguageInt == 0)
        {
            _continueText.text = "CONTINUE";
            _startText.text = "START NEW GAME";
            _optionText.text = "OPTIONS";
            _exitText.text = "EXIT";

            _difficultyText.text = "DIFFICULTY";
            _easyText.text = "EASY";
            _normalText.text = "NORMAL";
            _hardText.text = "HARD";
            _playText.text = "PLAY";
        }
        if(SettingMenu.LanguageInt == 1)
        {
            _continueText.text = "ПРОДОЛЖИТЬ";
            _startText.text = "НОВАЯ ИГРА";
            _optionText.text = "НАСТРОЙКИ";
            _exitText.text = "ВЫЙТИ";

            _difficultyText.text = "СЛОЖНОСТЬ";
            _easyText.text = "ЛЕГКАЯ";
            _normalText.text = "СРЕДНЯЯ";
            _hardText.text = "СЛОЖНАЯ";
            _playText.text = "ИГРАТЬ";
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Time.timeScale = 0;
    }

    private void OnApplicationFocus(bool focus)
    {
        Time.timeScale = 1;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        ChangeLanguageOfText();
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("MenuManager");

        writer.Write("LoadProgress", EnableContinue);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        QuickSaveWriter.Create("MenuManager");
        var reader = QuickSaveReader.Create("MenuManager");

        if (reader.Read<bool>("LoadProgress"))
        {
            _continueGame.SetActive(true);
            _continueGame.GetComponent<Button>().interactable = true;
        }
        else
        {
            _continueGame.GetComponent<Button>().interactable = false;
        }
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
        SaveCurrentProgress();
        StartCoroutine(LoadSceneAsync(sceneId));
    }


    public void ContinueGame()
    {
        Time.timeScale = 1;

        LoadProgress = true;
        EnableContinue = true;
        SaveCurrentProgress();
        LoadScene(3);
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        LoadProgress = false;
        EnableContinue = true;
        SaveCurrentProgress();
        LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
