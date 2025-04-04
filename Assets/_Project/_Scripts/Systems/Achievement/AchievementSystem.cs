using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : Singleton<AchievementSystem>
{
    [SerializeField] private List<AchievementAsset> _achievementList;
    
    private List<AchievementCondition> _achievementConditionList;

    private void Start()
    {
        _achievementConditionList = new List<AchievementCondition>(_achievementList.Count);

        for (int i = 0; i < _achievementList.Count; i++)
        {
            _achievementConditionList.Add(_achievementList[i].Condition);
        }
    }

    public void SucceedAchievement(AchievementCondition condition)
    {
        int index = _achievementConditionList.IndexOf(condition);

        _achievementList[index].IsUnlocked = true;

        Debug.Log("Unlock : " + _achievementList[index].Name);
        Debug.Log(_achievementList[index].Description);
    }

    
}

public enum AchievementCondition
{
    None,
    Two_Bubbles_In_Inventory,
}