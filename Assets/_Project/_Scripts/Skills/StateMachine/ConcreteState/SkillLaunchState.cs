using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkillLaunchState : SkillState
{
    public SkillLaunchState(PlayerSkillState playerSkill, SkillStateMachine skillStateMachine) : base(playerSkill, skillStateMachine)
    {
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (_playerSkill.EventPlayer.IsExitUseSkill == true)
        {
            _skillStateMachine.ChangeState(_playerSkill.LaunchState);
        }
    }

    public override void EnterState()
    {
        base.EnterState();
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
