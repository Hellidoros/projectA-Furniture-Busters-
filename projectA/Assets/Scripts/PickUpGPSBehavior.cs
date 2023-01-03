using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGPSBehavior : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-10.339f, -9.21f, 0.803f);
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();
        _handStateManager.CurrentInstance = Instantiate(_inventory.GPS, _handStateManager.ItemHolderLeft); ;
    }
}
