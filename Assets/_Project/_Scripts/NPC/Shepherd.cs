
using UnityEngine;

public class Shepherd : MonoBehaviour, ISpeakable
{
    [SerializeField] DialogueAsset _dialogue;

    public void Speak()
    {
        DialogueSystem.Instance.BeginDialogue(_dialogue);
    }
}
