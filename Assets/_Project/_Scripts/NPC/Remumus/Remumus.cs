using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remumus : MonoBehaviour, ISpeakable, IDetectable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] DialogueAsset _dryUnderwearDialogue;
    [SerializeField] DialogueAsset _orbDialogue;
    [SerializeField] bool _dryUnderwear = false;
    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorFeedback;
    [HideInInspector] public bool HasTakenOrbFragment = false;

    public bool DryUnderwear { get => _dryUnderwear; set => _dryUnderwear = value; }

    public void Speak()
    {
        _animator.SetBool("IsTalking", true);
        if (!_dryUnderwear && !HasTakenOrbFragment)
        {
            DialogueSystem.Instance.BeginDialogue(_basicDialogue);
        }
        else if (HasTakenOrbFragment)
        {
            DialogueSystem.Instance.BeginDialogue(_orbDialogue);
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
        if (DialogueSystem.Instance != null)
        {
            DialogueSystem.Instance.OnEndDialogue -= StopTalkAnimation;
        }
            
    }
}
