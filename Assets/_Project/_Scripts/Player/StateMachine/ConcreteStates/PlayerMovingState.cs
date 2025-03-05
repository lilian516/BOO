using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerMovingState : PlayerState
{
    public PlayerMovingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("je start moove");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //Debug.Log(_player.Input.GetMoveDirection());
        
        //Debug.Log("je FrameUpdate");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Debug.Log("je PhysicsUpdate");
    }

    public override void ChangeStateChecks() {
        base.ChangeStateChecks();
        if (_player.IsMoving() == false)
        {
            _playerStateMachine.ChangeState(_player.IdleState);

        }
    }
}
