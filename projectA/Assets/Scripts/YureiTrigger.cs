using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class YureiTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _monster;
    [SerializeField] private GameObject _virtualCam;
    [SerializeField] private GameObject _playerCam;
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private HandStateManager _handStateManager;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private AudioSource _audioSource;

    public GameObject PlayerItemsRendererCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<Collider>().enabled = false;
            StartCoroutine(Timer());
        }
    }


    public IEnumerator Timer()
    {
        _monster.SetActive(true);

        _playerCam.SetActive(false);
        _virtualCam.SetActive(true);

        _playerHealth.DisableCanvas();
        PlayerItemsRendererCamera.transform.parent = _virtualCam.transform;
        PlayerItemsRendererCamera.transform.position = _virtualCam.transform.position;
        PlayerItemsRendererCamera.transform.rotation = _virtualCam.transform.rotation;
        yield return new WaitForSeconds(3f);
        Destroy(_virtualCam);
        _handStateManager.ReEnableHand();
        _audioSource.Play();

        _playerCam.SetActive(true);

        PlayerItemsRendererCamera.transform.parent = _playerCam.transform;
        PlayerItemsRendererCamera.transform.position = _playerCam.transform.position;
        PlayerItemsRendererCamera.transform.rotation = _playerCam.transform.rotation;
        PlayerItemsRendererCamera.GetComponent<Camera>().enabled = true;

        _playerHealth.EnableCanvas();
        yield return new WaitForSeconds(0.5f);
        _handStateManager.ReEnableHand();
        _firstPersonController.CanMove = true;
        yield return new WaitForSeconds(1.5f);
        _monster.GetComponent<MotherMonsterScript>().enabled = true;
    }
}
