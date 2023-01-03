using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PlaceToHideChest : MonoBehaviour
{
    //Player
    private GameObject _playerCam;
    private FirstPersonController _firstPersonController;
    private PlayerHealth _playerHealth;
    private PlayerInteractor _playerInteractor;

    [SerializeField] private Transform _chestDoor;
    [SerializeField] private Vector3 _vector3;
    [SerializeField] private Vector3 _rotateVector3;
    [SerializeField] private Vector3 _positionAddVector3;

    private Vector3 _initialRotation;

    private bool _isHiding;

    private void Start()
    {
        _initialRotation =  _chestDoor.rotation.eulerAngles;

        _firstPersonController = PlayerReference.FirstPersonController;
        _playerCam = PlayerReference.FirstPersonController.gameObject;
        _playerHealth = PlayerReference.PlayerHealth;
        _playerInteractor = PlayerReference.PlayerInteractor;
    }

    private void Update()
    {
        if (_isHiding)
        {
            if (Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Interact") || Input.GetMouseButtonDown(0))
            {
                _chestDoor.DORotate(_initialRotation,1);

                _playerInteractor.enabled = true;

                _firstPersonController.enabled = true;
                _firstPersonController.gameObject.transform.position = this.transform.position + _vector3;
                _firstPersonController.gameObject.GetComponent<CharacterController>().enabled = true;

                _firstPersonController.CanMove = true;

                this.GetComponent<Interactable>().enabled = true;
                this.gameObject.layer = LayerMask.NameToLayer("Interactable");

                _playerHealth.StopHideEffect();
                _playerHealth.IsHiding = false;
                _isHiding = false;
            }
        }
    }

    public void HideInObject()
    {
        _chestDoor.DORotate(_rotateVector3,1);

        _firstPersonController.CanMove = false;

        _playerInteractor.enabled = false;

        _firstPersonController.enabled = false;
        _firstPersonController.gameObject.GetComponent<CharacterController>().enabled = false;
        _firstPersonController.transform.position = this.transform.position + _positionAddVector3;
        _playerCam.transform.localRotation = this.transform.rotation;

        this.GetComponent<Interactable>().enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Default");

        _playerHealth.StartHideEffect();
        StartCoroutine(StartHiding());
    }

    private IEnumerator StartHiding()
    {
        _firstPersonController.ReInitMouseLook();
        yield return new WaitForSeconds(0.1f);
        _firstPersonController.enabled = true;

        if (DiffucultyLevelScript.DifficultyLevel == 0)
        {
            _playerHealth.IsHiding = true;
        }
        yield return new WaitForSeconds(0.5f);
        _isHiding = true;
        if (DiffucultyLevelScript.DifficultyLevel == 1)
        {
            yield return new WaitForSeconds(0.5f);
            _playerHealth.IsHiding = true;
        }
        else if (DiffucultyLevelScript.DifficultyLevel == 2)
        {
            yield return new WaitForSeconds(2f);
            _playerHealth.IsHiding = true;
        }
    }
}
