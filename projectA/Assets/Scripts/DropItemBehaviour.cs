using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemBehaviour : StateMachineBehaviour
{
    private HandStateManager _handStateManager;
    private Inventory _inventory;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _inventory = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<Inventory>();
        _handStateManager = animator.GetComponent<HandStateManager>();

        _handStateManager.CurrentItemHolder.SetActive(true);
        _handStateManager.CurrentItemHolder.transform.parent = null;
        _handStateManager.CurrentItemHolder = null;
    }
}
