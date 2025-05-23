using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    public int comboCounter { get; private set; }

    private float lastTimeAttacked;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(2); // attack sound effect

        // xInput = 0;  // we need this to fix bug on attack direction

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + player.comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);


        // float attackDir = player.facingDir;
        //
        // if (xInput != 0)
        //     attackDir = xInput;
        

        player.SetVelocity(player.attackMovement[comboCounter].x * player.attackDir, player.attackMovement[comboCounter].y);


        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();
        if (triggerCalled || stateTimer < -0.5f)
        {
            stateMachine.ChangeState(player.idleState);
            comboCounter++;
            lastTimeAttacked = Time.time;
        }
    }

    
}
