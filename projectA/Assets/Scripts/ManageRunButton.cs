using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageRunButton : MonoBehaviour
{
    private bool _isRunnig = true;

    [SerializeField] private Sprite _run;
    [SerializeField] private Sprite _walk;

    [SerializeField] private Image _buttonImage;

    private void Start()
    {
        _isRunnig = true;
    }

    public void ManageButton()
    {
        _isRunnig = !_isRunnig;

        if (_isRunnig)
        {
            _buttonImage.sprite = _run;
        }
        else
        {
            _buttonImage.sprite = _walk;
        }
    }
}
