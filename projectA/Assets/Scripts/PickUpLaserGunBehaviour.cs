using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLaserGunBehaviour : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-10.121f, -17.463f, 2.271f);
        animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-3.906f, 8.139f, -3.607f);
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();

        if (_handStateManager.CurrentInstance != null)
            Destroy(_handStateManager.CurrentInstance);

        _handStateManager.CurrentInstance = Instantiate(_handStateManager.CurrentItem.Prefab, _handStateManager.ItemHolder); ;
    }
}
