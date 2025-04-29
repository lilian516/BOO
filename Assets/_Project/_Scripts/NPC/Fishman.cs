using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishman : MonoBehaviour, ISpeakable, IChangeable, IDetectable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorFeedback;

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
        _animator.SetBool("IsBooMad",true);
    }

    public void ResetChange()
    {
        _animator.SetBool("IsBooMad", false);
    }

    public void Detected()
    {
        if (!AngrySystem.Instance.IsAngry)
        {
            _animatorFeedback.SetBool("IsDetected", true);
            return;
        }
        NoDetected();
    }

    public void NoDetected()
    {
        _animatorFeedback.SetBool("IsDetected", false);
    }

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements -= Change;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnEndDialogue -= StopTalkAnimation;
        }
        
    }
}
