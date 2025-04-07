using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour, ISpeakable
{

    [SerializeField] DialogueAsset _dialogue;
    [SerializeField] Animator _animator;

    public void Speak()
    {
        //CinematicSystem.Instance.PlayCinematic("Min");
        _animator.SetBool("IsTalking", true);
        DialogueSystem.Instance.BeginDialogue(_dialogue);
        DialogueSystem.Instance.OnTakeEvent += OnEventTakeSkill;
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogueSystem.Instance.OnEndDialogue += StopTalkAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEventTakeSkill(DialogueEventType type)
    {
        switch (type)
        {
            case DialogueEventType.GetBubble:
                _animator.SetTrigger("HasTakeBubble");
                break;
            case DialogueEventType.GetWindmill:
                break;
        }
        _dialogue = _dialogue.NextDialogue;
        DialogueSystem.Instance.OnTakeEvent -= OnEventTakeSkill;
    }

    private void StopTalkAnimation()
    {
        _animator.SetBool("IsTalking", false);
    }
}
