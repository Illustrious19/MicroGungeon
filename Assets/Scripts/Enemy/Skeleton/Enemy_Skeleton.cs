using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy_Skeleton : Enemy
{

    #region States

    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }

    public SkeletonBlockedState blockedState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Battle", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        blockedState = new SkeletonBlockedState(this, stateMachine, "Blocked", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }


    public override bool CanBeBlocked()
    {
        if (base.CanBeBlocked())
        {
            stateMachine.ChangeState(blockedState);
            return true;
        }

        return false;
    }
    
    public override bool CanBeStunned(int AttackCount)
    {
        if (base.CanBeStunned(AttackCount))
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

    }
}
