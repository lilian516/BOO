using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillState : PlayerState
{
    public SkillStateMachine StateMachine { get; set; }
    public SkillLaunchState LaunchState { get; set; }

    public AnimEventPlayer EventPlayer { get; set; }


    public PlayerSkillState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        StateMachine = new SkillStateMachine();
        LaunchState = new SkillLaunchState(this, StateMachine);
        EventPlayer = _player.EventPlayer;
    }

    public override void EnterState()
    {
        base.EnterState();

        StateMachine.Initialize(LaunchState);

        if (!_player.UseCurrentSkill()) {
            _playerStateMachine.ChangeState(_player.IdleState);
        }
            
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        StateMachine.CurrentState.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        StateMachine.CurrentState.PhysicsUpdate();

    }



    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
        StateMachine.CurrentState.ChangeStateChecks();


        if (_player.EventPlayer.IsExitUseSkill == true)
        {
            if (_player.IsMoving() == false)
            {
                _playerStateMachine.ChangeState(_player.IdleState);

            }
            else
            {
                _playerStateMachine.ChangeState(_player.MovingState);
            }
        }
    }

   
}
