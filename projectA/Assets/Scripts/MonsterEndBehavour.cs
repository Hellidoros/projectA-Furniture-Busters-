using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEndBehavour : StateMachineBehaviour
{
    private EndDemoScript EndDemo;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EndDemo = animator.GetComponent<EndDemoScript>();
        EndDemo.Knife.SetActive(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EndDemo = animator.GetComponent<EndDemoScript>();
        EndDemo.EndFrame.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
