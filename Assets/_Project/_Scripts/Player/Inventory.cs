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
    private Sprite _baseButtonSprite;

    private CanvasGroup _skillCanvaGroup;

    private bool _achievementObtained;

    void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

    }

    public void Init()
    {
        _skillImages.Clear();
        _currentSkill = null;
        _skillCanvaGroup = GameManager.Instance.InventoryUI.GetComponent<CanvasGroup>();
        _skillButtonUI = GameManager.Instance.SkillStickUI.gameObject;
        _skillButtonUI.GetComponent<Animator>().enabled = false;
        _baseButtonSprite = _skillButtonUI.GetComponent<Image>().sprite;


        for (int i = 0; i < _skillCanvaGroup.transform.childCount; i++)
        {
            GameObject skillUI = _skillCanvaGroup.transform.GetChild(i).gameObject;
            _skillImages.Add(skillUI);
            Helpers.HideCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());

            int index = i;
            skillUI.GetComponent<Button>().onClick.AddListener(delegate { SelectSkill(index); ChangeSkillImage(); });

        }
        for (int i = 0; i < _skills.Count; i++)
        {
            Helpers.ShowCanva(_skillImages[i].gameObject.GetComponent<CanvasGroup>());
            _skillImages[i].GetComponent<Image>().sprite = _skills[i].GetSprite();
        }
        if(_skills.Count > 0)
        {
            Helpers.ShowCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());
        }

        if (AngrySystem.Instance.IsAngry)
            Change();
    }

    void Update()
    {
    }

    private void SelectSkill(int index)
    {
       ChangeCurrentSkill(_skills[index]);
    }

    private void ChangeSkillImage()
    {
        if (AngrySystem.Instance.IsAngry && _currentSkill != null)
        {
            _skillButtonUI.GetComponent<Image>().sprite = _angrySkills.GetSprite();
            return;
        }

        if (_currentSkill != null)
            _skillButtonUI.GetComponent<Image>().sprite = _currentSkill.GetSprite();
    }

    private void ChangeCurrentSkill(Skill skill)
    {
        _currentSkill = skill;
    }

    public void AddSkill(Skill skill, PlayerSkill playerSkill, bool isAngry = false)
    {
        if (_skills.Count == 3)
        {
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
        Player player = GameManager.Instance.Player.GetComponent<Player>();

        player.StateMachine.ChangeState(player.WaitingState);

        InputManager.Instance.DisableControllerStick();
        InputManager.Instance.DisableSkillStick();

        GameObject inventoryMenu = GameManager.Instance.InventoryFullMenu;
        Helpers.ShowCanva(inventoryMenu.GetComponent<CanvasGroup>());

        inventoryMenu.transform.Find("NewSkill").GetComponent<Image>().sprite = skill.GetSprite();

        for(int i = 0; i < _skills.Count; i++)
        {
            inventoryMenu.transform.Find("Skill"+ (i + 1)).GetComponent<Image>().sprite = _skills[i].GetSprite();
            int index = i;
            inventoryMenu.transform.Find("Skill"+ (i + 1)).GetComponent<Button>().onClick.AddListener(delegate { ReplaceSkill(index, skill, playerSkill); });
        }

        _currentSkill = null;

    }

    private void StopManageInventory()
    {
        Player player = GameManager.Instance.Player.GetComponent<Player>();

        player.StateMachine.ChangeState(player.TakeSkillState);

        InputManager.Instance.EnableControllerSticks();
        InputManager.Instance.EnableSkillStick();
        Helpers.HideCanva(GameManager.Instance.InventoryFullMenu.GetComponent<CanvasGroup>());

        for (int i = 0; i < _skills.Count; i++)
        {
            int index = i;
            GameManager.Instance.InventoryFullMenu.transform.Find("Skill" + (i + 1)).GetComponent<Button>().onClick.RemoveAllListeners();
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
        Helpers.ShowCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());
        _skillButtonUI.GetComponent<Animator>().enabled = true;
        _skillButtonUI.GetComponent<Animator>().SetTrigger("UseSmash");

        for (int i = 0; i < _skills.Count; i++)
        {
            Helpers.HideCanva(_skillCanvaGroup.transform.GetChild(i).gameObject.GetComponent<CanvasGroup>());
        }

        _currentSkill = _angrySkills;

        InputManager.Instance.OnSkillButton += delegate
        {
            _skillButtonUI.GetComponent<Animator>().SetTrigger("UseSmash");
        };

    }

    public void ResetChange()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            Helpers.ShowCanva(_skillCanvaGroup.transform.GetChild(i).gameObject.GetComponent<CanvasGroup>());
        }

        if (_skills.Count == 0)
        {
            Helpers.HideCanva(GameManager.Instance.SkillStickParent.GetComponent<CanvasGroup>());
        }

        _currentSkill = null;
        _skillButtonUI.GetComponent<Animator>().enabled = false;
        _skillButtonUI.GetComponent<Image>().sprite = _baseButtonSprite;

        InputManager.Instance.OnSkillButton -= delegate
        {
            _skillButtonUI.GetComponent<Animator>().SetTrigger("UseSmash");
        };
    }

    public void SetAngrySkill(Skill skill)
    {
        _angrySkills = skill;
    }

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements -= Change;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }
}
