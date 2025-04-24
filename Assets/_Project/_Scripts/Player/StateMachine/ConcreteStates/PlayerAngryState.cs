using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAngryState : PlayerState
{

    [System.Serializable]
    public class Descriptor
    {
        public AnimationClip AnimationAngry;
        public float DurationStop;
    }

    Descriptor _desc;



    public PlayerAngryState(Player player, PlayerStateMachine playerStateMachine, Descriptor desc) : base(player, playerStateMachine)
    {
        _desc = desc;
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
            _player.StateMachine.ChangeState(_player.IdleState);
            return;
        }

        AngrySystem.Instance.OnChangeElements += OnIdle;
        AngrySystem.Instance.ChangeAngryLimits();
        _player.PlayerAnimator.SetTrigger("Angry");
        _player.PlayerFaceAnimator.enabled = false;
        _player.PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;

       

        _player.OnEndAnimation += OnIdle;
        _player.WaitForSkillAnimation();

    }

    public override void ExitState()
    {
        base.ExitState();
        if (!AngrySystem.Instance.IsAngry)
            _player.PlayerFaceAnimator.enabled = true;

        AngrySystem.Instance.OnChangeElements -= OnIdle;
        _player.OnEndAnimation -= OnIdle;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void OnIdle() {
        _stateMachine.ChangeState(_player.IdleState);
    }

   
}
