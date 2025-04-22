
using Unity.VisualScripting;
using UnityEngine;

public class Shepherd : MonoBehaviour, ISpeakable
{
    [SerializeField] private SheepDetector _detector;

    #region Speak 
    [SerializeField] private DialogueAsset _dialogue;
    [SerializeField] private DialogueAsset _happyDialogue;
    [SerializeField] private DialogueAsset _sadDialogue;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        
    }

    void Update()
    {
        if (AngrySystem.Instance != null)
            _animator.SetBool("IsAfraid", AngrySystem.Instance.IsAngry);

        if (GameManager.Instance.KilledSheep >= 3)
            _animator.SetBool("IsSad", true);
    }

    public void Speak()
    {


        if (AngrySystem.Instance.IsAngry)
            return;


        DialogueSystem.Instance.OnEndDialogue += StopTalkAnimation;
        DialogueSystem.Instance.OnTakeEvent += OnEventTakeSkill;

        _animator.SetBool("IsSpeaking", true);
        if (GameManager.Instance.KilledSheep >= 3)
        {
            DialogueSystem.Instance.BeginDialogue(_sadDialogue);
        }
        else if (_detector.SheepCount >= 3)
        {
            DialogueSystem.Instance.BeginDialogue(_happyDialogue);
        }
        else
        {
            DialogueSystem.Instance.BeginDialogue(_dialogue);
        }
    }

    private void OnEventTakeSkill(DialogueEventType type)
    {
        switch (type)
        {
            case DialogueEventType.GetStick:
                _animator.SetTrigger("GiveStick");
                break;
            default:
                break;
        }
        _dialogue = _dialogue.NextDialogue;
        DialogueSystem.Instance.OnTakeEvent -= OnEventTakeSkill;
    }

    private void StopTalkAnimation()
    {
        _animator.SetBool("IsSpeaking", false);
        DialogueSystem.Instance.OnEndDialogue -= StopTalkAnimation;
    }

    #endregion

    #region States

    //public enum EShepherdStates
    //{
    //    Idle,
    //    Happy,
    //    Sad
    //}

    //private EShepherdStates _state;


    //private int _sheepCount;
    //public int _SheepCount { 
    //    get
    //    {
    //        return _sheepCount;
    //    }
    //    set
    //    {
    //        _sheepCount = value;

    //        if (_sheepCount == 0)
    //            ChangeState(EShepherdStates.Sad);
    //        else if (_sheepCount != 0)
    //            ChangeState(EShepherdStates.Happy);
    //    }
    //}

    //private void ChangeState(EShepherdStates state)
    //{
    //    // switch case 
    //    _state = state;
    //}
    #endregion
}
