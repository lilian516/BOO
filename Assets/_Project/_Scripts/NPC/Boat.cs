using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IInteractable
{
    private bool _hasInteractedWithStick = false;
    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.StickSkill:
                Debug.Log("A interagi avec le b�ton");
                _hasInteractedWithStick = true;
                break;
            case PlayerSkill.PantsSkill:
                if (_hasInteractedWithStick)
                {
                    Debug.Log("A plac� le slip sur le m�t");
                }
                else
                {
                    Debug.Log("Il semblerait qu'un slip dans un bateau ne soit pas d'une grande utilit�...");
                }
                break;
        }
    }
}
