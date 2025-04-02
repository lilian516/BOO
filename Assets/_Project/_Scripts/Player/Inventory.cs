using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
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

    public void OpenInventory()
    {
        if (AngrySystem.Instance.IsAngry || _skills.Count <= 1)
            return;

        _skillCanvaGroup.alpha = 1;
        _skillCanvaGroup.interactable = true;
        _skillCanvaGroup.blocksRaycasts = true;
        

    }

    public void CloseInventory()
    {
        if (AngrySystem.Instance.IsAngry || _skills.Count <= 1)
            return;

        if (InputManager.Instance.GetSelectDirection() != Vector2.zero && _skillCanvaGroup.alpha == 1)
        {
            for(int i = 0; i < _skillImages.Count; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(_skillImages[i].GetComponent<Button>().GetComponent<RectTransform>(), InputManager.Instance.GetTouchPosition()))
                {
                    ChangeCurrentSkill(_skills[i]);
                }
            }

        }
        _skillCanvaGroup.alpha = 0;
        _skillCanvaGroup.interactable = false;
        _skillCanvaGroup.blocksRaycasts = false;
        
    }
    private void ChangeCurrentSkill(Skill skill)
    {
        _currentSkill = skill;
    }

    public void AddSkill(Skill skill, PlayerSkill playerSkill)
    {
        if (_skills.Count == 2)
        {
            Time.timeScale = 0;
            OpenInventory();
            ManageInventory(skill, playerSkill);
            return;
        }

        PlayerSkills.Add(playerSkill);
        _skills.Add(skill);
        _currentSkill = skill;

        _skillCanvaGroup.transform.GetChild(_skills.Count - 1).gameObject.SetActive(true);
    }

    public void RemoveSkill(PlayerSkill skill)
    {
        int index = PlayerSkills.IndexOf(skill);

        if (_currentSkill == _skills[index])
        {
            _currentSkill = null;
        }
        _skillCanvaGroup.transform.GetChild(index).gameObject.SetActive(false);
        _skills.RemoveAt(index);
        PlayerSkills.Remove(skill);
    }

    private void ManageInventory(Skill skill, PlayerSkill playerSkill)
    {
        InputManager.Instance.DisableSticksAndButtons();
        GameManager.Instance.UIBackground.SetActive(true);

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
        CloseInventory();
        GameManager.Instance.UIBackground.SetActive(false);

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
        PlayerSkills.Remove(playerSkill);

        _skills.Insert(index,skill);
        PlayerSkills.Insert(index, playerSkill);

        

        StopManageInventory();
    }
}
