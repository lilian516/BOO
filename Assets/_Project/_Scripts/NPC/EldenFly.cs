using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EldenFly : MonoBehaviour, IInteractable
{
    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.PantsSkill:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        transform.Translate((GameManager.Instance.Player.transform.position - transform.position).normalized * 2.0f * Time.deltaTime);
    }
}
