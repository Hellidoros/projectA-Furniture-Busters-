using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class ManageCurrentScene : MonoBehaviour
{
    public static int CurrentSceneSave;
    [SerializeField] private bool _isSchoolFinished;
    [SerializeField] private bool _isForestFinished;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _ambientSoundTwo;

    [SerializeField] private bool _gotAxe = false;
    [SerializeField] private bool _gotPicture = false;

    [SerializeField] private bool _gotYellowKey = false;

    //FirstScenes

    public GameObject[] GameObjectsToDestroy;
    public GameObject[] GameoBjectsToSetActive;
    public GameObject[] Doors;


    //SchoolScene
    [SerializeField] private GameObject _axe;
    [SerializeField] private GameObject _puzzlePart;
    public GameObject[] GameObjectsToDisableAfterSchool;

    //Forest Scene
    [SerializeField] private GameObject _yellowKey;

    //Monster
    [SerializeField] private GameObject _monster;

    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        //SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        if (MenuManager.LoadProgress)
        {
            GetSavedReferences();
        }

        StartCoroutine(Timer());
    }

    public void GotAxe()
    {
        _gotAxe = true;
        SaveCurrentProgress();
    }

    public void GotPicture()
    {
        _gotPicture = true;
        SaveCurrentProgress();
    }

    public void GotYellowKey()
    {
        _gotYellowKey = true;
        SaveCurrentProgress();
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(2.5f);

        if (_isSchoolFinished) //If School Scene is finished
        {
            if (_puzzlePart != null)
            {
                if (!_gotPicture)
                {
                    _puzzlePart.SetActive(true);
                }
            }

            if (_axe != null)
            {
                if (!_gotAxe)
                {
                    _axe.SetActive(true);
                    _axe.GetComponent<Rigidbody>().isKinematic = true;
                    _axe.GetComponent<Rigidbody>().useGravity = false;
                }
                else
                {
                    _axe.GetComponent<Rigidbody>().isKinematic = false;
                    _axe.GetComponent<Rigidbody>().useGravity = true;
                }
            }


            foreach (GameObject gameObject in GameObjectsToDisableAfterSchool)
            {
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (_puzzlePart != null)
            {
                _puzzlePart.SetActive(false);
            }

            if (_axe != null)
            {
                _axe.SetActive(false);
            }

            foreach (GameObject gameObject in GameObjectsToDisableAfterSchool)
            {
                if(gameObject != null)
                {
                    gameObject.SetActive(true);
                }
            }
        }


        if (_isForestFinished)
        {
            if (!_gotYellowKey)
            {
                _yellowKey.SetActive(true);
            }
        }
        else
        {
            _yellowKey.SetActive(false);
        }

        if(_isSchoolFinished && !_isForestFinished)
        {
            _monster.SetActive(true);
        }
    }

    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        //SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("FirstScene");
        writer.Write("CurrentSceneSave", CurrentSceneSave);
        writer.Write("GotAxe", _gotAxe);
        writer.Write("GotPicture", _gotPicture);
        writer.Write("GotYellowKey", _gotYellowKey);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("FirstScene");

        CurrentSceneSave = reader.Read<int>("CurrentSceneSave");
        _isSchoolFinished = SchoolSceneManager.IsSceneFinished;
        _isForestFinished = ForestSceneManager.IsSceneFinished;

        _gotAxe = reader.Read<bool>("GotAxe");
        _gotPicture = reader.Read<bool>("GotPicture");
        _gotYellowKey = reader.Read<bool>("GotYellowKey");

        if(CurrentSceneSave == 1) //Saves Home scene
        {
            foreach (GameObject gameObject in GameObjectsToDestroy)
            {
                Destroy(gameObject);
            }

            foreach (GameObject gameObject in GameoBjectsToSetActive)
            {
                gameObject.SetActive(true);
            }

            foreach (GameObject door in Doors)
            {
                door.GetComponent<Door>().Locked = false;
                door.GetComponent<Renderer>().material.SetFloat("_VertexAnimationIntensity", 0f);
            }

            _audioSource.volume = 0.5f;
            _audioSource.clip = _ambientSoundTwo;
            _audioSource.Play();

            RenderSettings.fogDensity = 0.07f;
        }


    }
}

