using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;

    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySFX(14,enemy.transform);
    }

    public override void Update()
    {
        base.Update();


        if (stateTimer < 0)
        {
            if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            { 
                enemy.Flip();
            }
            stateMachine.ChangeState(enemy.moveState); 
        }
            
        
    }
}
