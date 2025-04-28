using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : Singleton<AchievementSystem>
{
    [SerializeField] private List<AchievementAsset> _achievementList;
    private List<bool> _achievementUnlocked;
    #region Achievement Variable
    public int PetCount = 0;
    #endregion

    private List<AchievementCondition> _achievementConditionList;

    [HideInInspector] public List<AchievementAsset> UIAchievementToUpdate;

    public void Init()
    {
        _achievementConditionList = new List<AchievementCondition>(_achievementList.Count);
        _achievementUnlocked = new List<bool>(_achievementList.Count);
        UIAchievementToUpdate = new List<AchievementAsset>(_achievementList.Count);

        for (int i = 0; i < _achievementList.Count; i++)
        {
            _achievementConditionList.Add(_achievementList[i].Condition);
            _achievementUnlocked.Add(SaveSystem.Instance.LoadElement<bool>(_achievementList[i].name));

            if (_achievementUnlocked[i])
                UIAchievementToUpdate.Add(_achievementList[i]);
        }
    }

    public void SucceedAchievement(AchievementCondition condition)
    {
        int index = _achievementConditionList.IndexOf(condition);

        if (_achievementUnlocked[index])
            return;

        _achievementUnlocked[index] = true;
        SaveSystem.Instance.SaveElement<bool>(_achievementList[index].name, true);

        GameObject panel = GameManager.Instance.UIAchievement;
        StartCoroutine(ShowAchievement(index, panel));
        StartCoroutine(MovePanel(-panel.transform.up));

        GameObject _list = GameManager.Instance.UIAchievementList;

        UIAchievementToUpdate.Add(_achievementList[index]);
    }
    
    private IEnumerator ShowAchievement(int index, GameObject panel)
    {
        panel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _achievementList[index].Name;
        
        yield return new WaitForSeconds(3);

        if (panel != null)
            StartCoroutine(MovePanel(panel.transform.up));
    }

    private IEnumerator MovePanel(Vector3 dir)
    {
        float elapsedTime = 0.1f;

        while (elapsedTime < 1)
        {
            if (GameManager.Instance.UIAchievement == null)
            {
                yield return null;
            }

            GameManager.Instance.UIAchievement.transform.Translate(dir * (100 / elapsedTime) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void PetAchievement()
    {
        if(PetCount == 10)
        {
            SucceedAchievement(AchievementCondition.Sheep_Lover);
        }
    }

}

public enum AchievementCondition
{
    None,
    Two_Bubbles_In_Inventory,
    Knock_At_The_Door,
    Sheep_Lover,
}