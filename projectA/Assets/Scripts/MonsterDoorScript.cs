using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;

public class MonsterDoorScript : MonoBehaviour
{
    [SerializeField] private Animator _monsterDoorAnimator;
    private HandStateManager _handStateManager;
    [SerializeField] private Interactable _interactable;
    [SerializeField] private NodeParser _nodeParser;
    private GameObject CurrentBanana;

    private bool IsActivated;

    void Start()
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
        var writer = QuickSaveWriter.Create("MonsterDoor");
        writer.Write("IsActivated", IsActivated);
        writer.Commit();
    }

    public void GetSavedReferences()
    {
        var reader = QuickSaveReader.Create("MonsterDoor");

        if (reader.Exists("IsActivated"))
        {
            IsActivated = reader.Read<bool>("IsActivated");
        }

        if (IsActivated)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            _interactable.enabled = false;
            _interactable.StopAllCoroutines();
            _monsterDoorAnimator.SetBool("isOpen", true);
        }
    }

    public void GetBanana()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "Banana")
            {
                _nodeParser.CanStartDialogue = false;
                CurrentBanana = _handStateManager.CurrentItemHolder;
                _monsterDoorAnimator.SetBool("isOpen", true);
                _handStateManager.DropItem();
                IsActivated = true;
                SaveCurrentProgress();
                if (_handStateManager.CurrentItemHolder != null)
                    Destroy(CurrentBanana);

                this.gameObject.layer = LayerMask.NameToLayer("Default");
                _interactable.enabled = false;
                _interactable.StopAllCoroutines();
            }
        }
    }
}
