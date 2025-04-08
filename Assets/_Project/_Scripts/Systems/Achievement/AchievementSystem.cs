using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSystem : Singleton<AchievementSystem>
{
    [SerializeField] private List<AchievementAsset> _achievementList;
    [SerializeField] private GameObject _achievementUIPrefab;
    
    private List<AchievementCondition> _achievementConditionList;

    private void Start()
    {
        _achievementConditionList = new List<AchievementCondition>(_achievementList.Count);

        for (int i = 0; i < _achievementList.Count; i++)
        {
            _achievementConditionList.Add(_achievementList[i].Condition);
        }
    }

    public void Init()
    {
        GameManager.Instance.UIAchievementButton.GetComponent<Button>().onClick.AddListener(ShowMenu);
        GameManager.Instance.UIAchievementMenu.GetComponentInChildren<Button>().onClick.AddListener(HideMenu);

        for (int i = 0; i < _achievementList.Count; i++)
        {
            GameObject prefab = Instantiate(_achievementUIPrefab, GameManager.Instance.UIAchievementList.transform);
            prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "?????";
            prefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _achievementList[i].Description;
        }
    }

    public void SucceedAchievement(AchievementCondition condition)
    {
        int index = _achievementConditionList.IndexOf(condition);

        _achievementList[index].IsUnlocked = true;

        GameObject panel = GameManager.Instance.UIAchievement;
        StartCoroutine(ShowAchievement(index, panel));
        StartCoroutine(MovePanel(-panel.transform.up, panel));

        GameObject _list = GameManager.Instance.UIAchievementList;

        _list.transform.GetChild(index).transform.GetChild(0).GetComponent<Image>().sprite = _achievementList[index].Sprite;
        _list.transform.GetChild(index).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _achievementList[index].Name;
        _list.transform.GetChild(index).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _achievementList[index].Description;
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
            panel.transform.Translate(dir * (100 / elapsedTime) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ShowMenu()
    {
        Helpers.ShowCanva(GameManager.Instance.UIAchievementMenu.GetComponent<CanvasGroup>());
    }

    private void HideMenu()
    {
        Helpers.HideCanva(GameManager.Instance.UIAchievementMenu.GetComponent<CanvasGroup>());
    }

}

public enum AchievementCondition
{
    None,
    Two_Bubbles_In_Inventory,
}