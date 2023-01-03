using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class ScreamerTrigger : MonoBehaviour
{
    public GameObject _screamerObject;
    private bool _isTriggerActivated;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartScreamer());
        }
    }

    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("Screamer");
        writer.Write("IsActivated", _isTriggerActivated);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("Screamer");
        _isTriggerActivated = reader.Read<bool>("IsActivated");

        if (_isTriggerActivated)
        {
            Destroy(this.gameObject);
        }
    }


    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public IEnumerator StartScreamer()
    {
        RenderSettings.fogDensity = 0.30f;
        _screamerObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        RenderSettings.fogDensity = 0.15f;
        _isTriggerActivated = true;
        SaveCurrentProgress();
        Destroy(_screamerObject);
        Destroy(this);
    }
}
