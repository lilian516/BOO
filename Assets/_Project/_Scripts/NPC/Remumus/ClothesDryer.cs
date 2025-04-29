using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesDryer : MonoBehaviour, IInteractable
{
    [SerializeField] Remumus _remumus;
    [SerializeField] Sprite _sprite;
    public void Interact(PlayerSkill playerSkill)
    {
        switch(playerSkill)
        {
            case PlayerSkill.WindSkill:
                _remumus.DryUnderwear = true;
                _remumus.GetAnimator().SetTrigger("TakeSlip");
                GetComponent<SpriteRenderer>().sprite = _sprite;
                break;
        }
    }
}
