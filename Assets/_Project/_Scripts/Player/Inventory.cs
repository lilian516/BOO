using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{

    private List<GameObject> _skillImages = new List<GameObject>();

    private List<Skill> _skills = new List<Skill>();

    private Skill _currentSkill;

    public Skill CurrentSkill { get => _currentSkill; set => _currentSkill = value; }

    void Start()
    {
        _skillImages.Add(GameObject.Find("ImageSkill1"));
        _skillImages.Add(GameObject.Find("ImageSkill2"));
        _skillImages.Add(GameObject.Find("ImageSkill3"));

        foreach(GameObject item in _skillImages)
        {
            item.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void OpenInventory()
    {
        for(int i = 0; i < _skillImages.Count; i++)
        {
            Debug.Log(_skillImages.Count);
            _skillImages[i].SetActive(true);
            int skillIndex = i;
            _skillImages[i].GetComponent<Button>().onClick.AddListener(() => ChangeCurrentSkill(_skills[skillIndex]));
        }
        
    }

    public void CloseInventory()
    {
        foreach (GameObject item in _skillImages)
        {
            item.SetActive(false);
        }
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
