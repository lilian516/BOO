using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Child : MonoBehaviour, ISpeakable
{

    [SerializeField] DialogueAsset _dialogue;
    [SerializeField] Animator _animator;
    [SerializeField] ChildEventPlayer _eventPlayer;
    [SerializeField] GameObject _windPrefab;
    Vector3 _launchDir;


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
        _eventPlayer.OnEndWindAnim += LaunchWind;

        _launchDir = new Vector3(1,0,-1);
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("IsBooMad", AngrySystem.Instance.IsAngry);
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
}
