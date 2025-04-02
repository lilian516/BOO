using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FliesSwarm : MonoBehaviour, IInteractable
{
    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.PantsSkill:
                Destroy(gameObject);
                break;
        }
    }
}
