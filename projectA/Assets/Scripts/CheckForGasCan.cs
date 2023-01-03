using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class CheckForGasCan : MonoBehaviour
{
    private HandStateManager _handStateManager;
    [SerializeField] private Animator _shelterAnimator;
    [SerializeField] private Material _shelterMaterial;
    [SerializeField] private BoxCollider _boxCollider;

    private bool IsIronBarOpened;

    private void Start()
    {
        SavePlayerProgress.SaveEvent += SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent += GetSavedReferences;

        _handStateManager = PlayerReference.HandStateManager;
    }

    public void OnDestroy()
    {
        SavePlayerProgress.SaveEvent -= SaveCurrentProgress;
        SavePlayerProgress.GetSavedProgressEvent -= GetSavedReferences;
    }

    public void SaveCurrentProgress()
    {
        var writer = QuickSaveWriter.Create("IronBar");

        writer.Write("IsOpen", IsIronBarOpened);

        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("IronBar");

        IsIronBarOpened = reader.Read<bool>("IsOpen");

        if (IsIronBarOpened)
        {
            OpenShelter();
        }
    }

    public void GetGasCan()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "GasCan")
            {
                _shelterMaterial.color = Color.green;
                _shelterAnimator.SetBool("isOpen", true);
                _boxCollider.enabled = false;
                _handStateManager.HandsAnimator.SetTrigger("MouseClicked");

                IsIronBarOpened = true;
                SaveCurrentProgress();
            }
        }
    }

    public void OpenShelter()
    {
        _boxCollider.enabled = false;
        _shelterAnimator.SetBool("isOpen", true);
    }
}
