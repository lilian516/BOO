using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Asset", menuName = "BOO/Achievement/Asset")]
public class AchievementAsset : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public AchievementCondition Condition;
}
