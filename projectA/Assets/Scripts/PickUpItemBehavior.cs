using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemBehavior : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;
    private Sway _sway;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-29.381f, -13.556f, -10.028f);
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();
        _sway = animator.GetComponent<Sway>();

        if(_sway != null)
        {
            _sway.OriginRotation = Quaternion.Euler(-29.381f, -13.556f, -10.028f);
        }

        if (_handStateManager.CurrentInstance != null)
        {
            Destroy(_handStateManager.CurrentInstance);
        }

        _handStateManager.CurrentInstance = Instantiate(_handStateManager.CurrentItem.Prefab, _handStateManager.ItemHolder); ;
    }
}
