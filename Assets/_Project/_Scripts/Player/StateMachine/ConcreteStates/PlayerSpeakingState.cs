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

        if(DialogueSystem.Instance.ProcessingDialogue.HasAnim)
            GameManager.Instance.Player.GetComponent<Player>().PlayerFaceAnimator.enabled = false;

        GameManager.Instance.Player.GetComponent<Player>().PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;

    }

    public override void ExitState()
    {
        GameManager.Instance.Player.GetComponent<Player>().PlayerFaceAnimator.enabled = true;
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

    public override void Destroy()
    {
        if (DialogueSystem.Instance != null)
        {
            _player.Input.OnSpeak -= DialogueSystem.Instance.AdvanceDialogue;
            DialogueSystem.Instance.OnChoice -= OnChoiceSelection;
            DialogueSystem.Instance.OnEndDialogue -= OnExitSpeakState;
        }
    }
}
