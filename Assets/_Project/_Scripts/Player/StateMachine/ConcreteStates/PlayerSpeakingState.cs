using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeakingState : PlayerState
{
    public PlayerSpeakingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();
    }

    public override void EnterState()
    {
        base.EnterState();
        _player.Input.OnSpeak += DialogueSystem.Instance.AdvanceDialogue;
        DialogueSystem.Instance.OnChoice += OnChoiceSelection;
        DialogueSystem.Instance.OnEndDialogue += OnExitSpeakState;

    }

    public override void ExitState()
    {
        _player.Input.OnSpeak -= DialogueSystem.Instance.AdvanceDialogue;
        DialogueSystem.Instance.OnChoice -= OnChoiceSelection;
        DialogueSystem.Instance.OnEndDialogue -= OnExitSpeakState;
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

    private void OnExitSpeakState()
    {
        _playerStateMachine.ChangeState(_player.IdleState);
    }

    private void OnChoiceSelection()
    {
        _player.Input.OnSpeak -= DialogueSystem.Instance.AdvanceDialogue;
    }
}
