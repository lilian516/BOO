using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillState : PlayerState
{
    public PlayerSkillState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _player.UseCurrentSkill();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
       
    }



    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
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
