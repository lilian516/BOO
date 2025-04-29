using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IInteractable
{
    private bool _hasInteractedWithStick = false;
    [SerializeField] Animator _animator;
    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.StickSkill:
                _hasInteractedWithStick = true;
                _animator.SetTrigger("Baton");
                break;
            case PlayerSkill.PantsSkill:
                if (_hasInteractedWithStick)
                {
                    _animator.SetTrigger("Slip");
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
