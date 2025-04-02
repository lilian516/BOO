
using UnityEngine;

public class Shepherd : MonoBehaviour, ISpeakable
{
    #region Speak 
    [SerializeField] DialogueAsset _dialogue;


    public void Speak()
    {
        DialogueSystem.Instance.BeginDialogue(_dialogue);
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
