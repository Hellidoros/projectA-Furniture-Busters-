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


        switch (SettingMenu.LanguageInt)
        {
            case 0: //English
                _continueText.text = "CONTINUE";
                _startText.text = "START NEW GAME";
                _optionText.text = "OPTIONS";
                _exitText.text = "EXIT";

                _difficultyText.text = "DIFFICULTY";
                _easyText.text = "EASY";
                _normalText.text = "NORMAL";
                _hardText.text = "HARD";
                _playText.text = "PLAY";
                break;

            case 1: //Russian
                _continueText.text = "ПРОДОЛЖИТЬ";
                _startText.text = "НОВАЯ ИГРА";
                _optionText.text = "НАСТРОЙКИ";
                _exitText.text = "ВЫЙТИ";

                _difficultyText.text = "СЛОЖНОСТЬ";
                _easyText.text = "ЛЕГКАЯ";
                _normalText.text = "СРЕДНЯЯ";
                _hardText.text = "СЛОЖНАЯ";
                _playText.text = "ИГРАТЬ";
                break;
            case 3: //Spanish
                _continueText.text = "CONTINÚE";
                _startText.text = "NUEVO JUEGO";
                _optionText.text = "CONFIGURACIÓN";
                _exitText.text = "OUT";

                _difficultyText.text = "CONECTAR";
                _easyText.text = "FÁCIL";
                _normalText.text = "MEDIO";
                _hardText.text = "CONECTAR";
                _playText.text = "PLAY";
                break;
            case 4: //German
                _continueText.text = "WEITERGEHEN";
                _startText.text = "NEUES SPIEL";
                _optionText.text = "EINSTELLUNGEN";
                _exitText.text = "EXIT";

                _difficultyText.text = "KONFIGURATION";
                _easyText.text = "EASY";
                _normalText.text = "MEDIUM";
                _hardText.text = "ADVANCED";
                _playText.text = "SPIELEN";
                break;
            case 5: //Japanese
                _continueText.text = "コンティニュー";
                _startText.text = "ニューゲーム";
                _optionText.text = "設定";
                _exitText.text = "エグジット";

                _difficultyText.text = "アドバンスド";
                _easyText.text = "EASY";
                _normalText.text = "MEDIUM";
                _hardText.text = "アドバンスド";
                _playText.text = "プレイ";
                break;
            case 6: //Portugese Brasilian
                _continueText.text = "CONTINUA";
                _startText.text = "NOVO JOGO";
                _optionText.text = "SETTINGS";
                _exitText.text = "SAÍDA";

                _difficultyText.text = "AVANÇADO";
                _easyText.text = "FÁCIL";
                _normalText.text = "MÉDIO";
                _hardText.text = "AVANÇADO";
                _playText.text = "JOGUE";
                break;
            case 7: //Korean
                _continueText.text = "계속";
                _startText.text = "새로운 게임";
                _optionText.text = "설정";
                _exitText.text = "EXIT";

                _difficultyText.text = "고급";
                _easyText.text = "쉬운";
                _normalText.text = "중간";
                _hardText.text = "고급";
                _playText.text = "플레이";
                break;
            case 8: //Indonesian
                _continueText.text = "LANJUTKAN";
                _startText.text = "PERMAINAN BARU";
                _optionText.text = "PENGATURAN";
                _exitText.text = "PENGATURAN";

                _difficultyText.text = "LANJUTAN";
                _easyText.text = "MUDAH";
                _normalText.text = "SEDANG";
                _hardText.text = "LANJUTAN";
                _playText.text = "BERMAIN";
                break;
            case 9: //Chinese easy
                _continueText.text = "继续";
                _startText.text = "新游戏";
                _optionText.text = "设置";
                _exitText.text = "退出";

                _difficultyText.text = "高级";
                _easyText.text = "简单";
                _normalText.text = "中等";
                _hardText.text = "高级";
                _playText.text = "玩耍";
                break;
            case 10: // Thai
                _continueText.text = "ดำเนินต่อ";
                _startText.text = "เกมส์ใหม่";
                _optionText.text = "การตั้งค่า";
                _exitText.text = "ออกจากระบบ";

                _difficultyText.text = "ความซับซ้อน";
                _easyText.text = "ง่าย";
                _normalText.text = "เฉลี่ย";
                _hardText.text = "ซับซ้อน";
                _playText.text = "เล่น";
                break;
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

        PlayerHealth.SetHeartsAmount(5);

        SaveCurrentProgress();
        LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
