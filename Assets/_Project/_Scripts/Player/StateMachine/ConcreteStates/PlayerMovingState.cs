using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;


public class PlayerMovingState : PlayerState
{

    [System.Serializable]
    public class Descriptor
    {
        public AnimationCurve SpeedCurve;
        public float Speed;
        public float DurationAcceleration;
    }

    Descriptor _desc;

    public PlayerMovingState(Player player, PlayerStateMachine playerStateMachine, Descriptor desc) : base(player, playerStateMachine)
    {
        _desc = desc;
    }
    private Vector3 _moveDirection;
    private float _time;
    private bool _facingRight = true;

    public override void EnterState()
    {
        base.EnterState();
        //_player.PlayerAnimator.SetFloat("Speed", 1);
        _player.PlayerAnimator.SetBool("IsMoving",true);
        _player.PlayerFaceAnimator.SetBool("IsMoving", true);
        _player.Input.OnSkillMenu += OnSkill;
        _time = 0;
        _desc.Speed = _player.CurrentSpeed;

        if (_player.transform.localScale.x == -1)
            _facingRight = false;

    }

    public override void ExitState()
    {
        base.ExitState();
        _player.PlayerAnimator.SetBool("IsMoving", false);
        _player.PlayerFaceAnimator.SetBool("IsMoving", false);
        _player.Input.OnSkillMenu -= OnSkill;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        _moveDirection = _player.GetMoveDirection();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        WalkingMove();

        float h = _player.RB.velocity.x;
        if (h > 0 && !_facingRight)
            Flip();
        else if (h < 0 && _facingRight)
            Flip();
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
        if (_time < _desc.DurationAcceleration)
        {
            float currentSpeed = _desc.SpeedCurve.Evaluate(_time / _desc.DurationAcceleration) * _desc.Speed;
            _time += Time.deltaTime;

            Vector3 vectorSpeed = _moveDirection * currentSpeed;
            vectorSpeed.y = 0; 
            _player.RB.velocity = vectorSpeed;
        }

        _player.LookDir = _moveDirection;

        if (_player.CanWalkForward)
        {
            Vector3 newVelocity = new Vector3(_moveDirection.x * _desc.Speed, _player.RB.velocity.y, _moveDirection.z * _desc.Speed);
            _player.RB.velocity = newVelocity;
        }
        else
        {
            _player.RB.velocity = Vector3.zero;
        }

        float animSpeed = Mathf.Abs(_player.RB.velocity.x) / 3 + Mathf.Abs(_player.RB.velocity.z) / 3;
        animSpeed *= _desc.Speed / 2;
        _player.PlayerAnimator.SetFloat("Speed", animSpeed);

        if (_player.RB.velocity.x < 0)
            Flip();
    }

    private void OnSkill()
    {
        _playerStateMachine.ChangeState(_player.SkillState);
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        
        Vector3 theScale = _player.transform.localScale;
        theScale.x *= -1;
        _player.transform.localScale = theScale;
    }
}
