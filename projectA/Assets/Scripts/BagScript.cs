using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField] private HandStateManager _handStateManager;
    [SerializeField] private TutorialScript _tutorialScript;
    public bool GetFlash;
    public bool GetTeddy;
    public bool GetPajama;

    private GameObject _currentItem;

    public void GetItems()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "Pajama")
            {
                GetPajama = true;
                if (_handStateManager.CurrentItemHolder != null)
                {
                    _currentItem = _handStateManager.CurrentItemHolder;
                    _handStateManager.DropItem();
                }

                Destroy(_currentItem);

                CheckForItems();
            }
            else if(_handStateManager.CurrentItem.Name == "Teddy")
            {
                GetTeddy = true;
                if (_handStateManager.CurrentItemHolder != null)
                {
                    _currentItem = _handStateManager.CurrentItemHolder;
                    _handStateManager.DropItem();
                }

                Destroy(_currentItem);

                CheckForItems();
            }
            else if (_handStateManager.CurrentItem.Name == "FlashLight")
            {
                GetFlash = true;
                if (_handStateManager.CurrentItemHolder != null)
                {
                    _currentItem = _handStateManager.CurrentItemHolder;
                    _handStateManager.DropItem();
                }

                Destroy(_currentItem);

                CheckForItems();
            }
        }
    }


    public void CheckForItems()
    {
        if(GetPajama && GetTeddy && GetFlash)
        {
            _tutorialScript.StartDialogueBool = true;
        }
    }

}
