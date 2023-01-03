using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using System;

public class SavePlayerProgress : MonoBehaviour
{
    [SerializeField] private GunShooting _gunShooting;
    [SerializeField] private Inventory _inventory;
    private int _currentPlayerGunAmmo;
    private int _currentAmmoStorage;
    public static event Action SaveEvent;
    public static event Action GetSavedProgressEvent;

    public bool getSavedProgress;

    public void Awake()
    {
        Application.quitting += SaveCurrentScene;
        getSavedProgress = MenuManager.LoadProgress;
    }

    public void GetReference()
    {

    }

    public void ClearSave()
    {

    }

    public void OnDestroy()
    {
        SaveCurrentScene();
        Application.quitting -= SaveCurrentScene;
    }

    public void OnDisable()
    {
        SaveCurrentScene();
    }

    public void OnEnable()
    {
        StartCoroutine(Timer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("Saved");
            //SaveProgress();
            SaveEvent?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log("Load");
            //SaveProgress();
            GetSavedProgressEvent?.Invoke();
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(2f);
        LoadPlayerProgress();
    }

    public void SaveCurrentScene()
    {
        SaveEvent?.Invoke();
    }

    public void LoadPlayerProgress()
    {
        if (getSavedProgress)
        {
            GetSavedProgressEvent?.Invoke();
        }
        else
        {
            SaveCurrentScene();
            MenuManager.LoadProgress = true;
        }
    }

}
