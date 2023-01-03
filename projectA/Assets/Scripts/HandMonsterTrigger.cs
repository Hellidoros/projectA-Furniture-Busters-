using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class HandMonsterTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool IsActivated;

    void Start()
    {
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
        var writer = QuickSaveWriter.Create("MonsterHand");

        writer.Write("IsActivated", IsActivated);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("MonsterHand");

        if (reader.Exists("IsActivated"))
        {
            IsActivated = reader.Read<bool>("IsActivated");
        }

        if (IsActivated)
        {
            Destroy(_animator.GetComponent<Transform>().gameObject);
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("Play", true);
            IsActivated = true;
            SaveCurrentProgress();
            Destroy(this.gameObject);
        }
    }
}
