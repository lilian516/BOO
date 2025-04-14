using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillState : PlayerState
{
    public SkillStateMachine StateMachine { get; set; }
    public SkillLaunchState LaunchState { get; set; }

    public AnimEventPlayer EventPlayer { get; set; }


    public PlayerSkillState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        StateMachine = new SkillStateMachine();
        LaunchState = new SkillLaunchState(this, StateMachine);
        EventPlayer = _player.EventPlayer;
    }

    public override void EnterState()
    {
        base.EnterState();

        //StateMachine.Initialize(LaunchState);

        if (!_player.StartUseCurrentSkill()) {
            _playerStateMachine.ChangeState(_player.IdleState);
        }
        _player.PlayerFaceAnimator.enabled = false;
        _player.PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;

    }

    public override void ExitState()
    {
        base.ExitState();
        _player.EventPlayer.IsExitUseSkill = false;
        if (!AngrySystem.Instance.IsAngry)
        {
            
            _player.PlayerFaceAnimator.enabled = true;
        }
       
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (_player.EventPlayer.IsUseSkill)
        {
            Debug.Log("on kill");
            _player.UseCurrentSkill();
            _player.EventPlayer.IsUseSkill = false;
        }
        //StateMachine.CurrentState.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //StateMachine.CurrentState.PhysicsUpdate();

    }



    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
        //StateMachine.CurrentState.ChangeStateChecks();


        if (_player.EventPlayer.IsExitUseSkill == true)
        {
            if (_player.IsMoving() == false)
            {
                _playerStateMachine.ChangeState(_player.IdleState);

            }
            else
            {
                _playerStateMachine.ChangeState(_player.MovingState);
            }

        }
    }

   
}
