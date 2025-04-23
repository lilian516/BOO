using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillState : PlayerState
{
    public SkillStateMachine StateMachine { get; set; }
    public SkillLaunchState LaunchState { get; set; }

    public AnimEventPlayer EventPlayer { get; set; }

    private SpriteRenderer[] _sprites;

    public PlayerSkillState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        StateMachine = new SkillStateMachine();
        LaunchState = new SkillLaunchState(this, StateMachine);
        EventPlayer = _player.EventPlayer;
    }

    public override void EnterState()
    {
        base.EnterState();

        _player.EventPlayer.OnEnterUseSkill += UseSkill;


        _sprites = _player.transform.GetComponentsInChildren<SpriteRenderer>();

        if (!_player.StartUseCurrentSkill()) {
            _playerStateMachine.ChangeState(_player.IdleState);
        }
        
        if (_player.HasSkillSelected())
        {
            if (Mathf.Sign(_player.LookDir.x) != Mathf.Sign(_player.SkillDir.x))
            {
                Flip(true);
            }
        }
        _player.OnEndAnimation += StopUseSkill;
        _player.WaitForSkillAnimation();
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.EventPlayer.OnEnterUseSkill -= UseSkill;
        _player.OnEndAnimation -= StopUseSkill;

        if (!AngrySystem.Instance.IsAngry)
        {
            
            _player.PlayerFaceAnimator.enabled = true;
        }
        Flip(false);

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

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
    }

    private void UseSkill()
    {
        _player.UseCurrentSkill();
    }

    private void StopUseSkill()
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

    private void Flip(bool flipped)
    {
        _sprites[0].flipX = flipped;
        _sprites[1].flipX = flipped;
    }
}
