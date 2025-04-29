using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    protected Player _player;
    
    protected PlayerStateMachine _playerStateMachine;
    
    public PlayerState(Player player, PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        _player = player;
        _playerStateMachine = playerStateMachine;
    }


    public override void EnterState() { }
    public override void ExitState() { }
    public override void FrameUpdate() { }
    public override void PhysicsUpdate() { }
    public override void ChangeStateChecks() { }
    public override void Destroy() { }
}
