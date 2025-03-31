using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Asset", menuName = "BOO/Dialogue/Asset")]
public class DialogueAsset : ScriptableObject
{
    [Header("Settings")]
    public bool OpeningTriggerEvent;
    public DialogueEventType OpeningEventType;
    public bool ClosureTriggerEvent;
    public DialogueEventType ClosureEventType;
    [Space]
    public DialogueSection[] Sections;
}

[System.Serializable]
public struct DialogueSection
{
    [TextArea] public string[] Sentences;
    public bool TriggerEvent;
    public DialogueEventType EventType;
}

public enum DialogueEventType
{
    None,
    GetUnderwear,
}
