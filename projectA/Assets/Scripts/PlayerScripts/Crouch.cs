using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;
    public bool IsCrouching = false;
    private float _originalHeight;
    [SerializeField] private float _crouchHeight = 0.5f;


    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _originalHeight = _characterController.height;
    }

    void Update()
    {
        if (Input.GetKeyDown(_crouchKey))
        {
            IsCrouching = true;
            _characterController.height = _crouchHeight;
        }
        else if (Input.GetKeyUp(_crouchKey))
        {
            IsCrouching = false;
            _characterController.height = _originalHeight;
        }
    }
}
