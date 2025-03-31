using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    public override void EnterState()
    {
        base.EnterState();
        _player.Input.OnUseSkill += OnSkill;
        _time = 0;


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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        WalkingMove();
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
        if(_time < _desc.DurationAcceleration)
        {
            float CurrentSpeed = _desc.SpeedCurve.Evaluate(_time/ _desc.DurationAcceleration) * _desc.Speed;

            _time += Time.deltaTime;
            Vector3 vectorSpeed = _moveDirection * CurrentSpeed; 
            vectorSpeed.y = vectorSpeed.y * 0;
            _player.RB.velocity = _moveDirection * CurrentSpeed;
        }

        _player.RB.velocity = _moveDirection * _desc.Speed;


    }

    private void OnSkill()
    {
        _playerStateMachine.ChangeState(_player.SkillState);
    }
}
