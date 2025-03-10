using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        _player.Input.OnUseSkill += OnSkill;
        
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.Input.OnUseSkill -= OnSkill;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //Debug.Log("je FrameUpdate");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Debug.Log("je PhysicsUpdate");
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();


        if (_player.IsMoving())
        {
            _playerStateMachine.ChangeState(_player.MovingState);
            
        }
        
    }

    private void OnSkill()
    {
        _playerStateMachine.ChangeState(_player.SkillState);
    }

}
