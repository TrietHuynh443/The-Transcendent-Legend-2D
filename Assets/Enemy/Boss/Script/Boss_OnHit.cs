using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_OnHit : StateMachineBehaviour
{
    private Rigidbody2D _rb;
    private Boss _boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rb = animator.GetComponent<Rigidbody2D>();
        _boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _boss.Move(Vector2.zero);

       if (_boss.IsWithInStrikingDistance){
            animator.SetTrigger("Attack1");
       } else {
            animator.SetTrigger("Walk");
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("OnHit");
    }
}
