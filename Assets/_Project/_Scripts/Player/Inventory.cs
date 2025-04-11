using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;


public class Inventory : MonoBehaviour, IChangeable
{
    public Skill CurrentSkill { get => _currentSkill; set => _currentSkill = value; }
    public List<PlayerSkill> PlayerSkills = new List<PlayerSkill>();

    private List<GameObject> _skillImages = new List<GameObject>();
    private List<Skill> _skills = new List<Skill>();
    private GameObject _skillButtonUI;

    private Skill _angrySkills;
    private Skill _currentSkill;

    private CanvasGroup _skillCanvaGroup;

    private bool _achievementObtained;

    void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

    }

    public void Init()
    {
        _skillCanvaGroup = GameManager.Instance.InventoryUI.GetComponent<CanvasGroup>();
        _skillButtonUI = GameManager.Instance.SkillStickParent.transform.GetChild(0).GetChild(0).gameObject;

        for (int i = 0; i < _skillCanvaGroup.transform.childCount; i++)
        {
            GameObject skillUI = _skillCanvaGroup.transform.GetChild(i).gameObject;
            _skillImages.Add(skillUI);
            Helpers.HideCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());

            EventTrigger eventTrigger;
            eventTrigger = skillUI.GetComponent<EventTrigger>();

            if (eventTrigger == null)
            {
                eventTrigger = skillUI.gameObject.AddComponent<EventTrigger>();
            }

            int index = i;
            EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
            pointerExitEntry.eventID = EventTriggerType.PointerExit;
            pointerExitEntry.callback.AddListener((eventData) => SelectSkill(index));

            eventTrigger.triggers.Add(pointerExitEntry);
        }
        for (int i = 0; i < _skills.Count; i++)
        {
            Helpers.ShowCanva(_skillImages[i].gameObject.GetComponent<CanvasGroup>());
        }
    }

    void Update()
    {
        
        
    }

    private void SelectSkill(int index)
    {
       if (AngrySystem.Instance.IsAngry)
       {
            ChangeCurrentSkill(_angrySkills);
            _skillButtonUI.GetComponent<Image>().sprite = _skills[index].GetSprite();
            return;
       }
        _skillButtonUI.GetComponent<Image>().sprite = _skills[index].GetSprite();
       ChangeCurrentSkill(_skills[index]);
    }

    private void ChangeCurrentSkill(Skill skill)
    {
        _currentSkill = skill;
    }

    public void AddSkill(Skill skill, PlayerSkill playerSkill, bool isAngry = false)
    {
        if (isAngry)
        {
            _angrySkills = skill;
            return;
        }
        if (_skills.Count == 7)
        {
            Time.timeScale = 0;
            ManageInventory(skill, playerSkill);
            return;
        }
        if (_skills.Count == 0)
        {
            Helpers.ShowCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());
        }

        if ( !_achievementObtained)
        {
            VerifyAchivement(playerSkill);
        }

        PlayerSkills.Add(playerSkill);
        _skills.Add(skill);

        GameObject inventoryItem = _skillCanvaGroup.transform.GetChild(_skills.Count - 1).gameObject;

        Helpers.ShowCanva(inventoryItem.GetComponent<CanvasGroup>());
        inventoryItem.GetComponent<Image>().sprite = skill.GetSprite();
    }

    public void RemoveSkill(PlayerSkill skill)
    {
        int index = PlayerSkills.IndexOf(skill);

        if (_currentSkill == _skills[index])
        {
            _currentSkill = null;
        }
        GameObject inventoryItem = _skillCanvaGroup.transform.GetChild(index).gameObject;

        Helpers.ShowCanva(inventoryItem.GetComponent<CanvasGroup>());
        inventoryItem.GetComponent<Image>().sprite = null;

        _skills.RemoveAt(index);
        PlayerSkills.Remove(skill);
    }

    private void ManageInventory(Skill skill, PlayerSkill playerSkill)
    {
        InputManager.Instance.DisableSticksAndButtons();
        Helpers.ShowCanva(GameManager.Instance.UIBackground.GetComponent<CanvasGroup>());
        Helpers.ShowCanva(GameManager.Instance.InventoryFullMenu.GetComponent<CanvasGroup>());

        GameManager.Instance.InventoryFullMenu.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = skill.GetSprite();
        GameManager.Instance.InventoryFullMenu.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = skill.GetName();

        for (int i = 0; i < _skillCanvaGroup.transform.childCount; i++)
        {
            int index = i;
            _skillCanvaGroup.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate { ReplaceSkill(index,skill, playerSkill); });
        }
    }

    private void StopManageInventory()
    {
        Time.timeScale = 1;
        InputManager.Instance.EnableSticksAndButtons();
        Helpers.HideCanva(GameManager.Instance.UIBackground.GetComponent<CanvasGroup>());
        Helpers.HideCanva(GameManager.Instance.InventoryFullMenu.GetComponent<CanvasGroup>());

        for (int i = 0; i < _skillCanvaGroup.transform.childCount; i++)
        {
            _skillCanvaGroup.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private void ReplaceSkill(int index,Skill skill, PlayerSkill playerSkill)
    {
        if (_currentSkill == _skills[index])
            _currentSkill = skill;

        _skills.RemoveAt(index);
        PlayerSkills.RemoveAt(index);

        _skills.Insert(index,skill);
        PlayerSkills.Insert(index, playerSkill);

        _skillCanvaGroup.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = skill.GetSprite();

        StopManageInventory();
    }

    private void VerifyAchivement(PlayerSkill skill)
    {
        if (skill == PlayerSkill.BubbleSkill && PlayerSkills.Contains(skill))
        {
            AchievementSystem.Instance.SucceedAchievement(AchievementCondition.Two_Bubbles_In_Inventory);
            _achievementObtained = true;
        }
    }

    public void Change()
    {
        for(int i = 0; i < _skills.Count; i++)
        {
            _skillCanvaGroup.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = _angrySkills.GetSprite();
        }

        if (_skills.Count == 0)
        {
            Helpers.ShowCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());
            Helpers.ShowCanva(_skillCanvaGroup.transform.GetChild(0).gameObject.GetComponent<CanvasGroup>());
            _skillCanvaGroup.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = _angrySkills.GetSprite();
        }

    }

    public void ResetChange()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _skillCanvaGroup.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = _skills[i].GetSprite();
        }

        if (_skills.Count == 0)
        {
            Helpers.HideCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());
            Helpers.HideCanva(_skillCanvaGroup.transform.GetChild(0).gameObject.GetComponent<CanvasGroup>());
            _skillCanvaGroup.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
        }
        
    }
}
