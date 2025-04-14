using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : State
{
    protected PlayerSkillState _playerSkill;

    protected SkillStateMachine _skillStateMachine;
    public SkillState(PlayerSkillState playerSkill, SkillStateMachine skillStateMachine) : base(skillStateMachine)
    {
        _playerSkill = playerSkill;
        _skillStateMachine = skillStateMachine;
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
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
