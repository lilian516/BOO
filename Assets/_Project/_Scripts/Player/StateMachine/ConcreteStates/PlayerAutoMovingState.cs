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

    private Vector3 _newPosition;
    private float _stoppingDistance;
    public PlayerAutoMovingState(Player player, PlayerStateMachine playerStateMachine, Descriptor desc) : base(player, playerStateMachine)
    {
        _desc = desc;
        _stoppingDistance = 0.25f;
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

        _newPosition = _player.PositionToGo;

        if (_newPosition.x < 0 && _player.LookDir.x > 0)
            Flip();
        else if (_newPosition.x > 0 && _player.LookDir.x < 0)
            Flip();

        _desc.NavMeshAgentPlayer.updateRotation = false;
        _desc.NavMeshAgentPlayer.enabled = true;
        _desc.NavMeshAgentPlayer.SetDestination(_newPosition);

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

    private void Flip()
    {
        Vector3 theScale = _player.transform.localScale;
        theScale.x *= -1;
        _player.transform.localScale = theScale;

        _player.LookDir = _newPosition.normalized;
    }
}
