using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishman : MonoBehaviour, ISpeakable, IChangeable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        DialogueSystem.Instance.OnEndDialogue += StopTalkAnimation;
    }

    public void Speak()
    {
        _animator.SetBool("IsSpeaking", true);
        DialogueSystem.Instance.BeginDialogue(_basicDialogue);
    }

    private void StopTalkAnimation()
    {
        _animator.SetBool("IsSpeaking", false);
    }

    public void Change()
    {
        Debug.Log("CHANGE WESH");
        _animator.SetBool("IsBooMad",true);
    }

    public void ResetChange()
    {
        _animator.SetBool("IsBooMad", false);
    }
}
