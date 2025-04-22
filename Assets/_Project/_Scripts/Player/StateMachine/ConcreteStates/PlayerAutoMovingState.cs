using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.ProBuilder.Shapes;
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
    private SpriteRenderer[] _sprites;
    public PlayerAutoMovingState(Player player, PlayerStateMachine playerStateMachine, Descriptor desc) : base(player, playerStateMachine)
    {
        _desc = desc;
        _stoppingDistance = 0.05f;
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

        _player.CurrentClickable.OnClick();

        if (!_player.CurrentClickable.CanGoTo)
        {
            _playerStateMachine.ChangeState(_player.IdleState);
            return;
        }

        _player.PlayerAnimator.SetBool("IsMoving", true);
        _player.PlayerFaceAnimator.SetBool("IsMoving", true);

        _newPosition = _player.PositionToGo;
        _sprites = _player.GetComponentsInChildren<SpriteRenderer>();
        Vector3 dir = _newPosition - _player.transform.position;
        if (dir.x > 0 && !_player.FacingRight)
            Flip(false);
        else if (dir.x < 0 && _player.FacingRight)
            Flip(true);

        _desc.NavMeshAgentPlayer.updateRotation = false;
        _desc.NavMeshAgentPlayer.enabled = true;

        CheckCanGoToPosition();





    }
    public override void ExitState()
    {
        base.ExitState();

        if (_player.CurrentClickable.CanGoTo)
            _player.CurrentClickable.OnDestinationReached();

        _desc.NavMeshAgentPlayer.enabled = false;
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

    private void Flip(bool flipped)
    {
        _player.FacingRight = !flipped;

        _sprites[0].flipX = flipped;
        _sprites[1].flipX = flipped;
    }

    private void CheckCanGoToPosition()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(_newPosition, out hit, _stoppingDistance, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (_desc.NavMeshAgentPlayer.CalculatePath(hit.position, path))
            {
                // Vérifie si le chemin est complet
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    _desc.NavMeshAgentPlayer.SetDestination(_newPosition);
                }
                else
                {
                    _stateMachine.ChangeState(_player.IdleState);
                }

            }

        }
        else
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
        
    }
}
