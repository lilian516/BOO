using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteractable
{
    private PlayerSkill _currentInteract;
    // Start is called before the first frame update
    void Start()
    {
        _currentInteract = PlayerSkill.None;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.BubbleSkill:
               
                PackedSheep();
                break;
        }
    }

    private void PackedSheep()
    {
        if(_currentInteract != PlayerSkill.BubbleSkill)
        {
            _currentInteract = PlayerSkill.BubbleSkill;
            Debug.Log("je suis embullé !!");
        }
        
    }
}
