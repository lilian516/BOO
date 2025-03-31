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
            //_skillImages[i].GetComponent<Button>().onClick.AddListener(() => CanChangeSkill(_skillImages[i].GetComponent<Button>()));

            EventTrigger eventTrigger = _skillImages[i].GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener((data) => { OnSkillOver((PointerEventData)data, _skills[skillIndex]); });
            eventTrigger.triggers.Add(entry);
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
        _skillCanvaGroup.alpha = 0;
        _skillCanvaGroup.interactable = false;
        _skillCanvaGroup.blocksRaycasts = false;
        
    }

    private void OnSkillOver(PointerEventData data, Skill skill)
    {
        ChangeCurrentSkill(skill);
    }

    private void ChangeCurrentSkill(Skill skill)
    {
        _currentSkill = skill;

        CloseInventory();
    }

    public void AddSkill(Skill skill)
    {
        _skills.Add(skill);
        Debug.Log(_skills.Count);
        _currentSkill = skill;
    }
}
