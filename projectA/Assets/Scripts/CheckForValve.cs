using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForValve : MonoBehaviour
{
    public Material ValveMaterial;
    [SerializeField] private MainDoorScript _mainDoor;
    private HandStateManager _handStateManager;
    private GameObject CurrentValve;

    private void Start()
    {
        _handStateManager = PlayerReference.HandStateManager;
    }

    public void GetValve()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "Valve")
            {
                this.GetComponent<MeshRenderer>().material = ValveMaterial;
                CurrentValve = _handStateManager.CurrentItemHolder;
                _handStateManager.DropItem();
                if(_handStateManager.CurrentItemHolder != null)
                    Destroy(CurrentValve);

                _mainDoor.CheckValve = true;
                _mainDoor.SaveCurrentProgress();
                _mainDoor.OpenMainDoor();
            }
        }
    }
}
