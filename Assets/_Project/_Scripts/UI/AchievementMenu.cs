using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenu : MonoBehaviour
{
    [SerializeField] private GameObject _achievementPrefab;
    [SerializeField] private Slider _scrollBar;
    private ScrollRect _scrollRect;

    private CanvasGroup _canvasGroup;
    private CanvasGroup _canvasMainMenuGroup;

    void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetMainMenuCanvaGroup(CanvasGroup canvasGroup)
    {
        _canvasMainMenuGroup = canvasGroup;
    }

    public void OpenAchievement(Animator animator)
    {
        animator.SetTrigger("ClickTrophy");

        List<AchievementAsset> achievements = AchievementSystem.Instance.UIAchievementToUpdate;
        for (int i = 0; i < achievements.Count; i++)
        {
            Destroy(GameManager.Instance.UIAchievementList.transform.GetChild(i).gameObject);
            GameObject achievement = Instantiate(_achievementPrefab, GameManager.Instance.UIAchievementList.transform);
            achievement.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = achievements[i].Name;
            achievement.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = achievements[i].Description;
            achievement.transform.Find("Logo").GetComponent<Image>().sprite = achievements[i].Sprite;
        }

        StartCoroutine(WaitOpenAchievement());
    }

    private IEnumerator WaitOpenAchievement()
    {
        
        yield return new WaitForSeconds(0.55f);

        Helpers.ShowCanva(_canvasGroup);
        Helpers.HideCanva(_canvasMainMenuGroup);
    }

    public void CloseAchievement(Animator animator)
    {
        animator.SetTrigger("Click");

        StartCoroutine(WaitCloseAchievement());

    }

    private IEnumerator WaitCloseAchievement()
    {
        yield return new WaitForSeconds(0.40f);

        Helpers.HideCanva(_canvasGroup);
        Helpers.ShowCanva(_canvasMainMenuGroup);
    }

    public void MatchScrollBarValue()
    {
        if (_scrollBar.value == _scrollRect.normalizedPosition.y)
            return;

        _scrollBar.value = _scrollRect.normalizedPosition.y;
    }

    public void MatchScrollRectValue()
    {
        if (_scrollBar.value == _scrollRect.normalizedPosition.y)
            return;

        _scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x,_scrollBar.value);
    }

}
