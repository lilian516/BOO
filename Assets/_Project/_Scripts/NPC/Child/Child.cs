using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Child : MonoBehaviour, ISpeakable, IChangeable, IDetectable
{

    [SerializeField] DialogueAsset _dialogue;
    [SerializeField] Animator _animator;
    [SerializeField] ChildEventPlayer _eventPlayer;
    [SerializeField] GameObject _windPrefab;
    [SerializeField] Animator _animatorFeedback;
    Vector3 _launchDir;


    public void Speak()
    {
        _animator.SetBool("IsTalking", true);
        DialogueSystem.Instance.BeginDialogue(_dialogue);
        DialogueSystem.Instance.OnTakeEvent += OnEventTakeSkill;
        DialogueSystem.Instance.OnEndDialogue += delegate { DialogueSystem.Instance.OnTakeEvent -= OnEventTakeSkill; };
    }

    void Start()
    {
        DialogueSystem.Instance.OnEndDialogue += StopTalkAnimation;
        _eventPlayer.OnEndWindAnim += LaunchWind;

        _launchDir = new Vector3(1,0,-1);

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    void Update()
    {
    }

    private void OnEventTakeSkill(DialogueEventType type)
    {
        switch (type)
        {
            case DialogueEventType.GetBubble:
                _animator.SetTrigger("HasTakeBubble");
                _animator.SetBool("HasTakenWind", false);
                break;
            case DialogueEventType.GetWindmill:
                _animator.SetTrigger("HasTakeBubble");
                _animator.SetBool("HasTakenWind", true);
                break;
        }
        _dialogue = _dialogue.NextDialogue;
        DialogueSystem.Instance.OnTakeEvent -= OnEventTakeSkill;
    }

    private void StopTalkAnimation()
    {
        _animator.SetBool("IsTalking", false);
    }

    private void LaunchWind()
    {
        GameObject wind = GameManager.Instance.SpawnObject(_windPrefab);
        wind.GetComponent<Wind>().Init(_launchDir, Quaternion.LookRotation(_launchDir,Vector3.up));

        wind.transform.position = transform.position;
    }

    public void Change()
    {
        _animator.SetBool("IsBooMad", true);
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
