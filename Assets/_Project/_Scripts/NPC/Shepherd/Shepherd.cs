
using Unity.VisualScripting;
using UnityEngine;

public class Shepherd : MonoBehaviour, ISpeakable
{
    [SerializeField] private SheepDetector _detector;

    #region Speak 
    [SerializeField] private DialogueAsset _dialogue;
    [SerializeField] private DialogueAsset _happyDialogue;
    [SerializeField] private DialogueAsset _sadDialogue;


    public void Speak()
    {
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

    #endregion

    #region States

    public enum EShepherdStates
    {
        Idle,
        Happy,
        Sad
    }

    private EShepherdStates _state;


    private int _sheepCount;
    public int _SheepCount { 
        get
        {
            return _sheepCount;
        }
        set
        {
            _sheepCount = value;

            if (_sheepCount == 0)
                ChangeState(EShepherdStates.Sad);
            else if (_sheepCount != 0)
                ChangeState(EShepherdStates.Happy);
        }
    }

    private void ChangeState(EShepherdStates state)
    {
        // switch case 
        _state = state;
    }
    #endregion
}
