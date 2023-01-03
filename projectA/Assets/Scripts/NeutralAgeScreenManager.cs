using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using UnityEngine.UI;
using TMPro;

public class NeutralAgeScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _neutralAgeScreen;
    [SerializeField] private TextMeshProUGUI _ageText;
    [SerializeField] private Slider _ageSlider;

    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TextMeshProUGUI _titleText;
    private bool _disaleNeutralAgeScreen = false;
    public static bool DisableProduct = false;

    private bool _enableAcceptButton = false;

    private void Awake()
    {
        if (Application.systemLanguage == SystemLanguage.Russian)
        {
            _buttonText.text = "ПРИНЯТЬ";
            _titleText.text = "УКАЖИТЕ СВОЙ ВОЗРАСТ";
        }
        if (Application.systemLanguage == SystemLanguage.English)
        {
            _buttonText.text = "ACCEPT";
            _titleText.text = "HOW OLD ARE YOU?";
        }

        GetSavedReferences();
        if (_disaleNeutralAgeScreen)
        {
            _neutralAgeScreen.SetActive(false);
        }
    }

    public void SetAgeNumber(float index)
    {
        _ageText.text = index.ToString();
        _enableAcceptButton = true;
    }

    public void CheckAge()
    {
        if (_enableAcceptButton)
        {
            if (_ageSlider.value > 0)
            {
                _disaleNeutralAgeScreen = true;

                if (_ageSlider.value <= 13)
                {
                    DisableProduct = true;
                }

                _neutralAgeScreen.SetActive(false);
                SaveCurrentProgress();
            }
        }
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("NeutralAgeManager");

        writer.Write("DisableProduct", DisableProduct);
        writer.Write("CloseAgeScreen", _disaleNeutralAgeScreen);

        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("NeutralAgeManager");

        DisableProduct = reader.Read<bool>("DisableProduct");
        _disaleNeutralAgeScreen = reader.Read<bool>("CloseAgeScreen");
    }
}
