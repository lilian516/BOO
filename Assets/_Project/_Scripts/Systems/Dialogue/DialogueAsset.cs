using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Asset", menuName = "BOO/Dialogue/Asset")]
public class DialogueAsset : ScriptableObject
{
    [Header("Settings")]
    public bool OpeningTriggerEvent;
    public DialogueEventType OpeningEventType;
    public bool ClosureTriggerEvent;
    public DialogueEventType ClosureEventType;
    [Header("Sprites")]
    public Sprite RightCharacter;
    public Sprite LeftCharacter;
    public Sprite DialogueBackground;
    [Space]
    public DialogueSection[] Sections;
    [Space]
    public bool ChoiceOnEnd;
    public DialogueEventType TakeEventType;
    public DialogueAsset NextDialogue;

    [Space]
    [Header("Skill Info")]
    public PlayerSkill SkillToGive;
    public SkillDescriptor SkillDescriptor;
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
    GetBubble,
    GetWindmill,
    EndDialogue
}
