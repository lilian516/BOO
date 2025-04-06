using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesDryer : MonoBehaviour, IInteractable
{
    [SerializeField] Remumus _remumus;
    public void Interact(PlayerSkill playerSkill)
    {
        Debug.Log("on interact");
        switch(playerSkill)
        {
            case PlayerSkill.WindSkill:
                Debug.Log("c'est le vent");
                _remumus.DryUnderwear = true;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
