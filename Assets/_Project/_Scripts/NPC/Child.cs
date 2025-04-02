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
        DialogueSystem.Instance.BeginDialogue(_dialogue);
        DialogueSystem.Instance.OnDialogueEvent += OnEventTakeSkill;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
            

    }
}
