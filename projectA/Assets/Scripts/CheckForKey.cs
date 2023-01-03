using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForKey : MonoBehaviour
{
    [SerializeField] private string _keyName;
    [SerializeField] private HandStateManager _handStateManager;
    [SerializeField] private Door _door;

    public void GetKey()
    {
        if(_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == _keyName)
            {
                _door.Locked = false;
            }
        }
    }
}
