using System.Collections.Generic;

using UnityEngine;
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
            _skillImages[i].GetComponent<Button>().onClick.AddListener(() => ChangeCurrentSkill(_skills[skillIndex]));
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
