using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeysLanguageManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interacrtText;
    [SerializeField] private TextMeshProUGUI _jumpText;
    [SerializeField] private TextMeshProUGUI _moveText;
    [SerializeField] private TextMeshProUGUI _inventoryText;


    void Start()
    {
        SettingMenu.ChangeLanguageEvent += ChangeLanguage;

        switch (SettingMenu.LanguageInt)
        {
            case 0: //English
                _interacrtText.text = "Interact";
                _jumpText.text = "Jump";
                _moveText.text = "Move";
                _inventoryText.text = "Inventory";
                break;
            case 1: //Russian
                _interacrtText.text = "Взаимодействие";
                _jumpText.text = "Прыжок";
                _moveText.text = "Двигаться";
                _inventoryText.text = "Инвентарь";
                break;

        }
    }

    private void OnDestroy()
    {
        SettingMenu.ChangeLanguageEvent -= ChangeLanguage;
    }

    public void ChangeLanguage()
    {
        switch (SettingMenu.LanguageInt)
        {
            case 0:
                _interacrtText.text = "Interact";
                _jumpText.text = "Jump";
                _moveText.text = "Move";
                _inventoryText.text = "Inventory";
                break;
            case 1:
                _interacrtText.text = "Взаимодействие";
                _jumpText.text = "Прыжок";
                _moveText.text = "Двигаться";
                _inventoryText.text = "Инвентарь";
                break;

        }
    }
}

