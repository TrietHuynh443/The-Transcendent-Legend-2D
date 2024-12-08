using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Attack2 : StateMachineBehaviour
{
    private Rigidbody2D _rb;
    private Boss _boss;

    [SerializeField] private float _bulletSpeed = 10f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _rb = animator.GetComponent<Rigidbody2D>();
        _boss = animator.GetComponent<Boss>();
        Attack();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _boss.Move(Vector2.zero);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private void Attack()
    {
        _boss.Move(Vector2.zero);
        float angleStep = 360f / 10;
        float angle = 0f;

        for (int i = 0; i < 10; i++)
        {
            float bulletDirX = _rb.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = _rb.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);
            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector2 bulletDir = (bulletMoveVector - _rb.transform.position).normalized;
            Bullet bullet = ObjectPooler.DequeueObject<Bullet>("Bullet");
            bullet.gameObject.SetActive(true);
            Collider2D collider = bullet.gameObject.GetComponent<Collider2D>();
            collider.enabled = true;
            bullet.Initialize();
            bullet.transform.position = _rb.transform.position;
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDir * _bulletSpeed;

            angle += angleStep;
        }
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
