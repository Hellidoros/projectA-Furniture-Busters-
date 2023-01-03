using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CI.QuickSave;

public class Door : MonoBehaviour
{
    [SerializeField] private Interactable _interactable;
    private NavMeshObstacle _obstacle;
    private Renderer _renderer;

    float targetYRotation;
    public bool Locked;

    public AudioClip _openDoor;
    public AudioClip _closeDoor;
    public AudioClip _closedDoor;
    private AudioSource _audioSource;

    public float smooth;
    public bool autoClose;

    public Transform player;

    float defaultYRotation = 0f;
    public float timer = 0f;

    public Transform pivot;

    public bool isOpen = false;
    public bool isMonster = false;

    private string _doorName;
    private Vector3 _currentMonsterPosition;

    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("Door" + _doorName);
        writer.Write("IsOpen", Locked);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        Debug.Log("Door");

        var reader = QuickSaveReader.Create("Door" + _doorName);

        if (reader.Exists("IsOpen"))
        {
            Locked = reader.Read<bool>("IsOpen");
        }

        if (!Locked)
        {
            var mats = _renderer.materials;

            if (mats.Length > 1)
            {
                mats[1].SetColor("_AlbedoColor", Color.black);
            }
        }
    }

    private void Awake()
    {
        _obstacle = GetComponent<NavMeshObstacle>();
        _obstacle.carveOnlyStationary = false;
        _obstacle.carving = isOpen;
        _obstacle.enabled = isOpen;

        _doorName = transform.parent.name;
    }

    void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        defaultYRotation = transform.eulerAngles.y;
        _audioSource = this.GetComponent<AudioSource>();

        _renderer = this.GetComponent<Renderer>();
    }

    void Update()
    {
        if (!Locked && !isMonster)
        {
            pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(0f, defaultYRotation + targetYRotation, 0f), smooth * Time.deltaTime);

            timer -= Time.deltaTime;

            if (timer <= 0f && isOpen && autoClose)
            {
                isMonster = false;
                ToggleDoor(player.position);
            }
        }
        if (isMonster)
        {
            pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(0f, defaultYRotation + targetYRotation, 0f), smooth * Time.deltaTime);
            timer -= Time.deltaTime;

            if (timer <= 0f && autoClose)
            {
                ToggleDoor(_currentMonsterPosition);
            }
        }
    }

    public void ToggleDoor(Vector3 pos)
    {
        isOpen = !isOpen;

        _obstacle.carving = isOpen;
        _obstacle.enabled = isOpen;

        if (isOpen)
        {
            Vector3 dir = (pos - transform.position);
            targetYRotation = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * 90f;
            timer = 5f;

            if (_audioSource != null)
            {
                _audioSource.clip = _openDoor;
                _audioSource.Play();
            }
        }
        else
        {
            targetYRotation = 0f;

            if (_audioSource != null)
            {
                _audioSource.clip = _closeDoor;
                _audioSource.Play();
            }
        }
    }

    public void Open(Vector3 pos)
    {
        if (!isOpen)
        {
            _currentMonsterPosition = pos;
            ToggleDoor(pos);
            if (_audioSource != null)
            {
                _audioSource.clip = _openDoor;
                _audioSource.Play();
            }
        }
    }
    public void Close(Vector3 pos)
    {
        if (isOpen)
        {
            ToggleDoor(pos);
            if (_audioSource != null)
            {
                _audioSource.clip = _closeDoor;
                _audioSource.Play();
            }
        }
    }

    public void Interact()
    {
        if (!Locked)
        {
            _interactable.Name = null;
            ToggleDoor(player.position);

            var mats = _renderer.materials;

            if (mats.Length > 1)
            {
                mats[1].SetColor("_AlbedoColor", Color.black);
            }
        }
        else
        {
            if(SettingMenu.LanguageInt == 0)
            {
                _interactable.Name = "Locked";
            }
            else if (SettingMenu.LanguageInt == 1)
            {
                _interactable.Name = "ЗАКРЫТО";
            }

            _audioSource.clip = _closedDoor;
            _audioSource.Play();
        }
    }

    public string GetDescription()
    {
        if (isOpen) return "Close the door";
        return "Open the door";
    }
}