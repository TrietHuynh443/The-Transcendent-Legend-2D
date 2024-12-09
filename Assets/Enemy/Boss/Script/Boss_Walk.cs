using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    private Transform _player;
    private Rigidbody2D _rb;
    private float _timeToSpawnBullet = 3f;
    [SerializeField] private float _chaseSpeed = 5f;
    private float _timer = 3f;
    Boss boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        _player = boss.PlayerTransform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        boss.LookAtPLayer();

        if (!boss.IsAggroed && _timer >= _timeToSpawnBullet)
        {
            _timer = 0f;
            animator.SetTrigger("Attack2");
        }
        else
        {
            Vector2 moveDirection = (_player.position - _rb.transform.position).normalized;
            boss.Move(moveDirection * _chaseSpeed);

            if (boss.IsWithInStrikingDistance)
            {
                animator.SetTrigger("Attack1");
            }
        }

        _timer += Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Walk");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
