using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    
    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }


    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void ChangeStateChecks() { }
}
