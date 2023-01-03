using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFlashLightBehavior : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;
    private Sway _sway;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-10.339f, -9.21f, 0.803f);
        animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-10.344f, -0.686f, -0.73f);
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();
        _sway = animator.GetComponent<Sway>();

        if(_sway != null)
        {
            _sway.OriginRotation = Quaternion.Euler(-10.344f, -0.686f, -0.73f);
        }

        if (_handStateManager.CurrentInstance != null)
        {
            Destroy(_handStateManager.CurrentInstance);
        }

        _handStateManager.CurrentInstance = Instantiate(_inventory.FlashLight, _handStateManager.ItemHolder); ;
    }
}
