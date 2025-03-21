using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class PlayerMovingState : PlayerState
{
    public PlayerMovingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    private Vector3 _moveDirection;

    public override void EnterState()
    {
        base.EnterState();
        _player.Input.OnUseSkill += OnSkill;
        Debug.Log("je start moove");
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.Input.OnUseSkill -= OnSkill;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _moveDirection = _player.GetMoveDirection();
        //Debug.Log(_player.Input.GetMoveDirection());

        //Debug.Log("je FrameUpdate");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        WalkingMove();
        //Debug.Log("je PhysicsUpdate");
    }



    public override void ChangeStateChecks() {
        base.ChangeStateChecks();
        if (_player.IsMoving() == false)
        {
            _playerStateMachine.ChangeState(_player.IdleState);

        }
    }

    private void WalkingMove()
    {
        _player.RB.AddForce(_moveDirection * _player.Speed * 1.5f, ForceMode.Force);
    }

    private void OnSkill()
    {
        _playerStateMachine.ChangeState(_player.SkillState);
    }
}
