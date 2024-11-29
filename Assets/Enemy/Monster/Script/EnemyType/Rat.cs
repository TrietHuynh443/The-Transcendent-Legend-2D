using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    private Vector2 _startPos;
    private bool _isTurnBack = false;
    [SerializeField] private GameObject _attackTrigger;

    public override void AttackStateHandle(ref float timer)
    {
        _attackTrigger.SetActive(true);
    }

    public override void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        // throw new System.NotImplementedException();
    }

    public override void DieStateHandle()
    {
    }

    public override void IdleStateHandle()
    {
        IsAggroed = true;
    }

    protected override void Start()
    {
        base.Start();
        _startPos = transform.position;
    }

    private void Update()
    {
        base.Update();
        if(Vector2.Distance(transform.position, _startPos) >= MoveRange)
        {
            _isTurnBack = true;
            _startPos = transform.position;
        }
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
        if(_isTurnBack)
        {
            _isTurnBack = false;
            TurnBack();
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 2f, LayerMask.NameToLayer("Ground"));
        if(hit.collider != null)
        {
            TurnBack();
            _startPos = transform.position;
        }
    }

    public override void Move(Vector2 velocity)
    {
        Rigidbody.velocity = velocity * transform.right;
    }

    public override void MoveStateHandle()
    {
        Move(new Vector2(MoveSpeed, 0));
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(transform.position, transform.position + transform.right * 1.5f);
    // }

    void OnTriggerEnter(Collider other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            Debug.Log("" + other.gameObject.name);
        }
    }
    
}
