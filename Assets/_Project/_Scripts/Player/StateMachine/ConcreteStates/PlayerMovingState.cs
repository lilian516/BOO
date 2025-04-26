using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
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

    private OnScreenStick _joystick;

    private SpriteRenderer[] _sprites;

    public override void EnterState()
    {
        base.EnterState();

        if (!AngrySystem.Instance.IsAngry)
        {
            _player.PlayerFaceAnimator.enabled = true;
        }

        _sprites = _player.GetComponentsInChildren<SpriteRenderer>();
        _player.PlayerAnimator.SetBool("IsMoving",true);
        _player.PlayerFaceAnimator.SetBool("IsMoving", true);
        _player.Input.OnSkillButton += OnSkill;
        _time = 0;
        _desc.Speed = _player.CurrentSpeed;

        _joystick = GameManager.Instance.GameController.GetComponentInChildren<OnScreenStick>();
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.PlayerAnimator.SetBool("IsMoving", false);
        _player.PlayerFaceAnimator.SetBool("IsMoving", false);
        _player.Input.OnSkillButton -= OnSkill;
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
        if (h > 0 && !_player.FacingRight)
            Flip(false);
        else if (h < 0 && _player.FacingRight)
            Flip(true);
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
            float Speed = _desc.Speed * _joystick.control.magnitude;
            Speed = Mathf.Clamp(Speed, _desc.Speed / 3, _desc.Speed);
            Vector3 newVelocity = new Vector3(_moveDirection.x * Speed, _player.RB.velocity.y, _moveDirection.z * Speed);
            _player.RB.velocity = newVelocity;
        }
        else
        {
            _player.RB.velocity = Vector3.zero;
        }

        float animSpeed = Mathf.Abs(_player.RB.velocity.x) / 3 + Mathf.Abs(_player.RB.velocity.z) / 3;
        animSpeed *= _desc.Speed / 2;
        animSpeed = Mathf.Clamp(animSpeed, 0.5f, 1);
        _player.PlayerAnimator.SetFloat("Speed", animSpeed);
        _player.PlayerFaceAnimator.SetFloat("Speed",animSpeed);

        Vector3 CapsulePos = _player.transform.position + _player.DirectionalCapsuleOffset;
        Vector3 Dir = InputManager.Instance.GetMoveDirection().normalized * 0.3f;
        _player.DirectionalCapsule.transform.position = new Vector3(Dir.x + CapsulePos.x, CapsulePos.y, CapsulePos.z + Dir.y);
    }

    private void OnSkill()
    {
        _playerStateMachine.ChangeState(_player.SkillState);
    }

    private void Flip(bool flipped)
    {
        _player.FacingRight = !_player.FacingRight;

        _sprites[0].flipX = flipped;
        _sprites[1].flipX = flipped;
    }
}
