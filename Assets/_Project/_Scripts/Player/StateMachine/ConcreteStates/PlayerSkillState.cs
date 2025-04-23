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

    private bool _isFlipped;

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
        _sprites = _player.transform.GetComponentsInChildren<SpriteRenderer>();
        _player.EventPlayer.OnEnterUseSkill += UseSkill;
        _player.EventPlayer.OnExitUseSkill += StopUseSkill;

        if (!_player.StartUseCurrentSkill()) {
            _playerStateMachine.ChangeState(_player.IdleState);
        }

        if (_player.HasSkillSelected())
        {
            if (_player.SkillDir.x > 0 && !_player.FacingRight)
                Flip(false);
            else if (_player.SkillDir.x < 0 && _player.FacingRight)
                Flip(true);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.EventPlayer.OnEnterUseSkill -= UseSkill;
        _player.EventPlayer.OnExitUseSkill -= StopUseSkill;

        if (!AngrySystem.Instance.IsAngry)
        {
            
            _player.PlayerFaceAnimator.enabled = true;
        }



    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

    }



    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
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
        _player.FacingRight = !flipped;

        _sprites[0].flipX = flipped;
        _sprites[1].flipX = flipped;
    }
}
