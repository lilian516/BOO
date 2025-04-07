using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    private List<GameObject> _skillImages = new List<GameObject>();

    private List<Skill> _skills = new List<Skill>();

    private Skill _currentSkill;

    private CanvasGroup _skillCanvaGroup;

    public List<PlayerSkill> PlayerSkills = new List<PlayerSkill>();
    public Skill CurrentSkill { get => _currentSkill; set => _currentSkill = value; }

    void Start()
    {
        
       
    }

    public void Init()
    {
        _skillCanvaGroup = GameManager.Instance.InventoryUI.GetComponent<CanvasGroup>();

        for (int i = 0; i < _skillCanvaGroup.transform.childCount; i++)
        {
            _skillImages.Add(_skillCanvaGroup.transform.GetChild(i).gameObject);
            _skillCanvaGroup.transform.GetChild(i).gameObject.SetActive(false);


        }
        for (int i = 0; i < _skills.Count; i++)
        {
            _skillImages[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public bool SelectSkill()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_skillImages[i].GetComponent<Button>().GetComponent<RectTransform>(), InputManager.Instance.GetTouchPosition()))
            {
                ChangeCurrentSkill(_skills[i]);
                return true ;
            }
        }
        return false;
    }

    private void ChangeCurrentSkill(Skill skill)
    {
        _currentSkill = skill;
    }

    public void AddSkill(Skill skill, PlayerSkill playerSkill)
    {
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

        PlayerSkills.Add(playerSkill);
        _skills.Add(skill);
        _currentSkill = skill;

        GameObject inventoryItem = _skillCanvaGroup.transform.GetChild(_skills.Count - 1).gameObject;

        inventoryItem.SetActive(true);
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

        inventoryItem.SetActive(false);
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
}
