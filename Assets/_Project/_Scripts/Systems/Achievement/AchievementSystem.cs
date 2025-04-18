using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : Singleton<AchievementSystem>
{
    [SerializeField] private List<AchievementAsset> _achievementList;

    #region Achievement Variable
    public int PetCount = 0;
    #endregion

    private List<AchievementCondition> _achievementConditionList;

    [HideInInspector] public List<Sprite> UIAchievementToUpdate;

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

        if (_achievementList[index].IsUnlocked)
            return;

        _achievementList[index].IsUnlocked = true;

        GameObject panel = GameManager.Instance.UIAchievement;
        StartCoroutine(ShowAchievement(index, panel));
        StartCoroutine(MovePanel(-panel.transform.up, panel));

        GameObject _list = GameManager.Instance.UIAchievementList;

        UIAchievementToUpdate.Add(_achievementList[index].Sprite);
    }
    
    private IEnumerator ShowAchievement(int index, GameObject panel)
    {
        panel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = _achievementList[index].Name;
        
        yield return new WaitForSeconds(3);

        StartCoroutine(MovePanel(panel.transform.up, panel));
    }

    private IEnumerator MovePanel(Vector3 dir, GameObject panel)
    {
        float elapsedTime = 0.1f;

        while (elapsedTime < 1)
        {
            if (panel == null)
                yield return null;

            panel.transform.Translate(dir * (100 / elapsedTime) * Time.deltaTime);
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