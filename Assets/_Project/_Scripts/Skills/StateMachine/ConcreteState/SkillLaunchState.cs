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

    public override void EnterState()
    {
        Debug.Log("on entre");
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        Debug.Log("on update");
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
