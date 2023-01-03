using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityStandardAssets.Characters.FirstPerson;

public class TriggerMonsterDissolve : MonoBehaviour
{
    [SerializeField] private DissolveMonster _dissolveMonster;
    [SerializeField] private GameObject _virtualCam;
    [SerializeField] private GameObject _monster;
    [SerializeField] private GameObject _jumpscare;

    //Player
    [SerializeField] private HandStateManager _handStateManager;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _playerCam;


    public GameObject[] Doors;

    public GameObject[] _monsters;

    public GameObject PlayerItemsRendererCamera;

    private bool triggerFinished = false;

    private void Start()
    {
        _handStateManager = PlayerReference.HandStateManager;
        _firstPersonController = PlayerReference.FirstPersonController;
        _playerHealth = PlayerReference.PlayerHealth;
        _playerCam = PlayerReference.PlayerCam;

        PlayerItemsRendererCamera = PlayerReference.PlayerItemsRendererCamera;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _handStateManager.ExitAllStates();
            StartCoroutine(StartMonsterDissolve());
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator StartMonsterDissolve()
    {
        _playerCam.SetActive(false);
        _virtualCam.SetActive(true);

        _playerHealth.DisableCanvas();

        _firstPersonController.CanMove = false;

        PlayerItemsRendererCamera.transform.parent = _virtualCam.transform;
        PlayerItemsRendererCamera.transform.position = _virtualCam.transform.position;
        PlayerItemsRendererCamera.transform.rotation = _virtualCam.transform.rotation;
        yield return new WaitForSeconds(3f);
        _dissolveMonster.StartDissolve = true;
        yield return new WaitForSeconds(2f);
        Destroy(_monster);
        Destroy(_virtualCam);
        _playerCam.SetActive(true);
        RenderSettings.fogDensity = 0.09f;
        PlayerItemsRendererCamera.transform.parent = _playerCam.transform;
        PlayerItemsRendererCamera.transform.position = _playerCam.transform.position;
        PlayerItemsRendererCamera.transform.rotation = _playerCam.transform.rotation;
        PlayerItemsRendererCamera.GetComponent<Camera>().enabled = true;

        _firstPersonController.CanMove = true;
        _playerHealth.EnableCanvas();
        yield return new WaitForSeconds(0.5f);
        _handStateManager.ReEnableHand();
        yield return new WaitForSeconds(10f);
        _jumpscare.SetActive(true);
        _playerHealth.Vignette.smoothness.value = _playerHealth.AlertVignette;
        RenderSettings.fogDensity = 0.15f;

        _handStateManager.ReEnableHand();

        _playerHealth.DisableCanvas();
        yield return new WaitForSeconds(1.8f);
        _playerHealth.EnableCanvas();
        _jumpscare.SetActive(false);
        _playerHealth.Vignette.smoothness.value = _playerHealth.NormalVignette;
        RenderSettings.fogDensity = 0.09f;

        ManageCurrentScene.CurrentSceneSave = 1;


        foreach(GameObject door in Doors)
        {
            door.GetComponent<Door>().Locked = false;
            door.GetComponent<Renderer>().material.SetFloat("_VertexAnimationIntensity", 0f);
        }

        foreach(GameObject monster in _monsters)
        {
            monster.SetActive(true);
        }
    }
}
