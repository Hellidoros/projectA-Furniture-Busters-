using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenBox : MonoBehaviour
{
    private bool _boxOpened;
    [SerializeField] private float _moveX;
    [SerializeField] private float _moveY;
    [SerializeField] private float _moveZ;
    [SerializeField] private float _duration = 1;
    private Vector3 _initialPosition;

    private void Start()
    {
        _boxOpened = false;
        _initialPosition = transform.localPosition;
    }

    public void ManageOpenBox()
    {
        if (!_boxOpened)
        {
            if(_moveX != 0)
            {
                transform.DOMoveX(_moveX, _duration);
            }
            if(_moveY != 0)
            {
                transform.DOMoveY(_moveY, _duration);
            }
            if (_moveZ != 0)
            {
                transform.DOMoveZ(_moveZ, _duration);
            }


            _boxOpened = true;
        }
        else
        {
            transform.DOMove(_initialPosition, _duration);
            _boxOpened = false;
        }
    }
}
