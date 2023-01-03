using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    private bool _doorOpened;
    [SerializeField] private float _rotateX;
    [SerializeField] private float _rotateY;
    [SerializeField] private float _rotateZ;
    [SerializeField] private float _duration = 1;
    private Vector3 _initialRotation;

    private void Start()
    {
        _doorOpened = false;
        _initialRotation = transform.localRotation.eulerAngles;
    }


    public void ManageOpenDoor()
    {
        if (!_doorOpened)
        {
            transform.DORotate(new Vector3(_rotateX, _rotateY, _rotateZ), _duration);

            _doorOpened = true;
        }
        else
        {
            transform.DORotate(_initialRotation, _duration);

            _doorOpened = false;
        }
    }
}
