using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillLaunchState : SkillState
{
    public SkillLaunchState(PlayerSkillState playerSkill, SkillStateMachine skillStateMachine) : base(playerSkill, skillStateMachine)
    {
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

    }

    private void EndSkillAnimation()
    {
        _skillStateMachine.ChangeState(_playerSkill.LaunchState);
    }

    public override void EnterState()
    {
        base.EnterState();

        _playerSkill.EventPlayer.OnExitUseSkill += EndSkillAnimation;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
