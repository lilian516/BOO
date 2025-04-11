using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillState : PlayerState
{


    public SkillStateMachine StateMachine { get; set; }
    public SkillLaunchState LaunchState { get; set; }
    
    public PlayerSkillState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        StateMachine = new SkillStateMachine();
        LaunchState = new SkillLaunchState(this, StateMachine);
    }

    public override void EnterState()
    {
        base.EnterState();

        StateMachine.Initialize(LaunchState);
        _player.UseCurrentSkill();
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

        
        AnimatorStateInfo stateInfo = _player.PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        
        if (stateInfo.IsTag("Skill") && stateInfo.normalizedTime >= 1f)
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
