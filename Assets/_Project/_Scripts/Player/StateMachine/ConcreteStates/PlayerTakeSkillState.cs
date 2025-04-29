using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeSkillState : PlayerState
{
    public PlayerTakeSkillState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
    }

    public override void EnterState()
    {
        base.EnterState();
        if(AngrySystem.Instance.IsAngry)
        {
            _playerStateMachine.ChangeState(_player.IdleState);
        }

        _player.EventPlayer.OnExitUseSkill += ExitTakeSkill;
        _player.PlayerAnimator.SetTrigger("TakeSkill");
        _player.PlayerFaceAnimator.enabled = false;
        _player.PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    public override void ExitState()
    {
        base.ExitState();

        _player.PlayerAnimator.SetTrigger("Idle");
        _player.EventPlayer.OnExitUseSkill -= ExitTakeSkill;
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

    private void ExitTakeSkill()
    {
        _stateMachine.ChangeState(_player.IdleState);
    }

    public override void Destroy()
    {
        _player.EventPlayer.OnExitUseSkill -= ExitTakeSkill;
    }
}
