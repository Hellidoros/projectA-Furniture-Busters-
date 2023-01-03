using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGreenKey : MonoBehaviour
{
    public string KeyName;
    public string GameObjectName;
    [SerializeField] private MainDoorScript _mainDoor;
    private HandStateManager _handStateManager;
    [SerializeField] private Vector3 _keyPosition;
    [SerializeField] private Quaternion _keyQuaternion;
    private GameObject CurrentKey;
    [SerializeField] private bool _addKey = true;

    private void Start()
    {
        _handStateManager = PlayerReference.HandStateManager;
    }

    public void GetKey()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == KeyName)
            {
                CurrentKey = _handStateManager.CurrentItemHolder;
                _handStateManager.DropItem();
                if (CurrentKey != null)
                {
                    CurrentKey.GetComponent<Transform>().parent = null;
                    CurrentKey.GetComponent<Transform>().position = _keyPosition;
                    CurrentKey.GetComponent<Transform>().rotation = _keyQuaternion;
                    CurrentKey.GetComponent<Rigidbody>().isKinematic = true;
                    CurrentKey.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == GameObjectName)
        {
            if (_addKey)
            {
                _mainDoor.KeyQuantity++;
            }
            _mainDoor.OpenMainDoor();
            _addKey = false;
        }
    }
}
