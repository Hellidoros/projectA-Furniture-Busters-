using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlateChangeLanguageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private string _english;
    [SerializeField] private string _russian;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.3f);
        ChangeLanguage();
    }

    public void ChangeLanguage()
    {
        if (SettingMenu.LanguageInt == 0)
        {
            _text.text = _english;
        }
        else if (SettingMenu.LanguageInt == 1)
        {
            _text.text = _russian;
        }
    }

}
