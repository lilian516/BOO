using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerAutoMovingState : PlayerState
{

    [System.Serializable]
    public class Descriptor
    {
        public NavMeshAgent NavMeshAgentPlayer;
    }

    Descriptor _desc;

    private Vector3 _NewPosition;
    private float _stoppingDistance;
    public PlayerAutoMovingState(Player player, PlayerStateMachine playerStateMachine, Descriptor desc) : base(player, playerStateMachine)
    {
        _desc = desc;
        _stoppingDistance = 0.5f;
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (_desc.NavMeshAgentPlayer.pathPending == false && _desc.NavMeshAgentPlayer.remainingDistance <= _stoppingDistance)
        {
            _playerStateMachine.ChangeState(_player.WaitingState);
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        _player.PlayerAnimator.SetBool("IsMoving", true);
        _player.PlayerFaceAnimator.SetBool("IsMoving", true);

        _NewPosition = _player.PositionToGo;

        _desc.NavMeshAgentPlayer.updateRotation = false;
        _desc.NavMeshAgentPlayer.enabled = true;
        _desc.NavMeshAgentPlayer.SetDestination(_NewPosition);

        //Debug.Log("j'entre dans auto moving state");
    }

    public override void ExitState()
    {
        base.ExitState();

        _desc.NavMeshAgentPlayer.enabled = false;
        _player.CurrentClickable.OnClick();
        _player.PlayerAnimator.SetBool("IsMoving", false);
        _player.PlayerFaceAnimator.SetBool("IsMoving", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
