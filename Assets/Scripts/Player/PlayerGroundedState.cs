using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpCounter = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
        // {
        //     if (player.skill.blackhole.cooldownTimer > 0)
        //     {
        //         player.fx.CreatePopUpText("Cooldown!");
        //         return;
        //     }
        //
        //
        //     stateMachine.ChangeState(player.blackHole);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
        //     stateMachine.ChangeState(player.aimSowrd);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
