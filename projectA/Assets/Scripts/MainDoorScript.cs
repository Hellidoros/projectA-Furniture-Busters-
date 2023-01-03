using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityStandardAssets.Characters.FirstPerson;
using CI.QuickSave;

public class MainDoorScript : MonoBehaviour
{
    public int KeyQuantity;
    public int PlankQuantity;
    public bool CheckValve;

    public Material ValveActiveMaterial;
    public MeshRenderer ValveMeshRenderer;

    public GameObject[] GameObjectsToDisable;

    public GameObject[] GameObjectsToSetActive;

    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private Animator _transition;
    [SerializeField] private NodeParser _nodeParser;
    [SerializeField] private Animator _monsterAnimator;

    //Player

    private GameObject _hands;
    private PlayerHealth _playerHealth;
    private FirstPersonController _firstPersonController;

    [SerializeField] private GameObject _camera1;
    [SerializeField] private GameObject _camera2;


    public void Start()
    {
        KeyQuantity = 0;
        PlankQuantity = 0;
        CheckValve = false;

        _hands = PlayerReference.HandStateManager.gameObject;
        _playerHealth = PlayerReference.PlayerHealth;
        _firstPersonController = PlayerReference.FirstPersonController;

        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;
    }

    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("MainDoor");

        writer.Write("Valve", CheckValve);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("MainDoor");

        if (reader.Exists("Valve"))
        {
            CheckValve = reader.Read<bool>("Valve");
        }

        if (CheckValve)
        {
            ValveMeshRenderer.material = ValveActiveMaterial;
        }
    }

    public void OpenMainDoor()
    {
        if(KeyQuantity >= 3 && PlankQuantity >= 2 && CheckValve)
        {
            foreach (GameObject gameObject in GameObjectsToDisable)
            {
                gameObject.SetActive(false);
            }

            _firstPersonController.CanMove = false;
            _firstPersonController.StopAllCoroutines();
            _playableDirector.Play();
            RenderSettings.fogDensity = 0.3f;
        }
    }

    public void NextCutScene()
    {
        _transition.SetBool("isEnded", true);
        _playerHealth.StopChasseEffect();
        StartCoroutine(PrepareNextScene());
    }

    IEnumerator PrepareNextScene()
    {
        foreach(GameObject gameObject in GameObjectsToSetActive)
        {
            gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
        _camera1.SetActive(true);

        yield return new WaitForSeconds(2f);
        _transition.SetBool("isEnded", false);
        _firstPersonController.enabled = false;
        _firstPersonController.gameObject.GetComponent<CharacterController>().enabled = false;
        _firstPersonController.gameObject.transform.position = new Vector3(-7.785094f, -14.28f, -5.89f);
        yield return new WaitForSeconds(1f);
        _firstPersonController.enabled = true;
        _hands.SetActive(false);
        _nodeParser.StartDialogue();
    }

    public void StartMonsterAnimation()
    {
        _camera1.SetActive(false);
        _camera2.SetActive(true);
        _monsterAnimator.SetBool("Kill", true);
    }

}
