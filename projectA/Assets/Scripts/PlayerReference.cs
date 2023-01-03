using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerReference : MonoBehaviour
{
    [SerializeField] private FirstPersonController _fpController;
    [SerializeField] private GameObject _playerCam;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerInteractor _playerInteractor;
    [SerializeField] private HandStateManager _handStateManager;
    [SerializeField] private GameObject _playerItemsRendererCamera;
    [SerializeField] private GameObject _mainCam;

    public static FirstPersonController FirstPersonController;
    public static GameObject PlayerCam;
    public static PlayerHealth PlayerHealth;
    public static PlayerInteractor PlayerInteractor;
    public static HandStateManager HandStateManager;
    public static GameObject PlayerItemsRendererCamera;
    public static GameObject MainCamera;

    private void Awake()
    {
        FirstPersonController = _fpController;
        PlayerCam = _playerCam;
        PlayerHealth = _playerHealth;
        PlayerInteractor = _playerInteractor;
        HandStateManager = _handStateManager;
        PlayerItemsRendererCamera = _playerItemsRendererCamera;
        MainCamera = _mainCam;
    }
}
