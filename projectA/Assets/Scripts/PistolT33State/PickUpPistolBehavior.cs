using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPistolBehavior : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-10.121f, -17.463f, 2.271f);
        animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-7.567f, -2.03f, 4.242f);
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();

        if (_handStateManager.CurrentInstance != null)
            Destroy(_handStateManager.CurrentInstance);

        _handStateManager.CurrentInstance = Instantiate(_handStateManager.CurrentItem.Prefab, _handStateManager.ItemHolder); ;
    }
}
