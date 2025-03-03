using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        //Debug.Log("je start");
    }

    public override void ExitState()
    {
        base.ExitState();
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
}
