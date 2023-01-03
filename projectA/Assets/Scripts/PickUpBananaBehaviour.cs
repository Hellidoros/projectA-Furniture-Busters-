using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBananaBehaviour : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;
    private Sway _sway;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //previous settings
        //animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-10.339f, -9.21f, 0.803f);
        animator.GetComponent<Transform>().localRotation = Quaternion.Euler(-21.113f, 15.228f, -8.245f);
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();

        _sway = animator.GetComponent<Sway>();

        if (_sway != null)
        {
            _sway.OriginRotation = Quaternion.Euler(-21.113f, 15.228f, -8.245f);
        }

        if (_handStateManager.CurrentInstance != null)
            Destroy(_handStateManager.CurrentInstance);

        _handStateManager.CurrentInstance = Instantiate(_handStateManager.CurrentItem.Prefab, _handStateManager.ItemHolder); ;
    }
}
