using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    public State CurrentState;

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(State newState)
    {
        
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
