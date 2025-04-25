using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remumus : MonoBehaviour, ISpeakable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] DialogueAsset _dryUnderwearDialogue;
    [SerializeField] bool _dryUnderwear = false;
    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorFeedback;




    public bool DryUnderwear { get => _dryUnderwear; set => _dryUnderwear = value; }

    public void Speak()
    {
        _animator.SetBool("IsTalking", true);
        if (!_dryUnderwear)
        {
            DialogueSystem.Instance.BeginDialogue(_basicDialogue);
        }
        else
        {
            DialogueSystem.Instance.BeginDialogue(_dryUnderwearDialogue);
            DialogueSystem.Instance.OnEndDialogue += OnEventEndDialogue;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        DialogueSystem.Instance.OnEndDialogue += StopTalkAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        if (AngrySystem.Instance != null) 
            _animator.SetBool("IsBooAngry", AngrySystem.Instance.IsAngry);
    }

    private void OnEventEndDialogue()
    {
        if (_dryUnderwear)
        {
            _dryUnderwearDialogue = _dryUnderwearDialogue.NextDialogue;
        }
    }

    private void StopTalkAnimation()
    {
        _animator.SetBool("IsTalking", false);
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public void Detected()
    {
        _animatorFeedback.SetBool("IsDetected", true);
    }

    public void NoDetected()
    {
        _animatorFeedback.SetBool("IsDetected", false);
    }
}
