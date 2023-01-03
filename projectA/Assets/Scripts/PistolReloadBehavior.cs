using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolReloadBehavior : StateMachineBehaviour
{
    private GunShooting _gunShooting;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _gunShooting = animator.GetComponentInParent<Transform>().parent.GetComponentInParent<GunShooting>();
        _gunShooting.ReloadFinished = true;
        _gunShooting.Reloading = false;
    }


}
