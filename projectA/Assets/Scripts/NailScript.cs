using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailScript : MonoBehaviour
{
    [SerializeField] private WoodenPlankManager _woodenPlankManager;
    private HandStateManager _handStateManager;


    void Start()
    {
        _handStateManager = _woodenPlankManager.HandStateManager;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().useGravity = false;
    }

    public void DestroyNail()
    {
        if (_handStateManager.CurrentItem != null)
        {
            if (_handStateManager.CurrentItem.Name == "Crowbar")
            {
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                _handStateManager.HandsAnimator.SetTrigger("MouseClicked");

                _woodenPlankManager.AddOneNumber(1);
            }
        }
    }
}
