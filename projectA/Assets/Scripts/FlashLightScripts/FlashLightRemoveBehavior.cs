using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightRemoveBehavior : StateMachineBehaviour
{
    private HandStateManager _handStateManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _handStateManager = animator.GetComponent<HandStateManager>();
        animator.SetBool("isRunning", false);
        animator.ResetTrigger("MouseClicked");
        if (_handStateManager.CurrentInstance != null)
            Destroy(_handStateManager.CurrentInstance);
    }
}
