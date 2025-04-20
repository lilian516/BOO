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
                _hasInteractedWithStick = true;
                break;
            case PlayerSkill.PantsSkill:
                if (_hasInteractedWithStick)
                {
                    Debug.Log("A placé le slip sur le mât");
                }
                else
                {
                    Debug.Log("Il semblerait qu'un slip dans un bateau ne soit pas d'une grande utilité...");
                }
                break;
        }
    }
}
