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
        }
        for (int i = 0; i < _skillImages.Count; i++)
        {
            int skillIndex = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void OpenInventory()
    {

        _skillCanvaGroup.alpha = 1;
        _skillCanvaGroup.interactable = true;
        _skillCanvaGroup.blocksRaycasts = true;
        

    }

    public void CloseInventory()
    {
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

    public void AddSkill(Skill skill)
    {
        _skills.Add(skill);
        Debug.Log(_skills.Count);
        _currentSkill = skill;
    }
}
