using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;

    protected EnemyStateMachine EnemyStateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine EnemyStateMachine)
    {
        this.enemy = enemy;
        this.EnemyStateMachine = EnemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) { }
}
