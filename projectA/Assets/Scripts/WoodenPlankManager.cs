using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlankManager : MonoBehaviour
{
    [SerializeField] private MainDoorScript _mainDoor;
    public HandStateManager HandStateManager;
    public int Number;
    public int FallNumber;

    private void Start()
    {
        HandStateManager = PlayerReference.HandStateManager;
    }

    public void AddOneNumber(int num)
    {
        Number += num;
        if (Number > FallNumber)
        {
            _mainDoor.PlankQuantity++;
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.layer = LayerMask.NameToLayer("Interactable");
            _mainDoor.OpenMainDoor();
        }
    }

    public void DestroyPlank()
    {
        if (HandStateManager.CurrentItem != null)
        {
            if (HandStateManager.CurrentItem.Name == "Crowbar")
            {
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                HandStateManager.HandsAnimator.SetTrigger("MouseClicked");
                _mainDoor.PlankQuantity++;
                _mainDoor.OpenMainDoor();
            }
        }
    }


}
