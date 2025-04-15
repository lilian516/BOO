using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsSkill : Skill
{
    public PantsSkill(Player player, SkillDescriptor desc) : base(player)
    {
        _desc = desc;
    }

    public override void UseSkill()
    {
        Debug.Log("Slip");
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _desc.Radius, _desc.Mask);

        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(PlayerSkill.PantsSkill);

            }
            Debug.Log("Objet détecté slip : " + hitCollider.gameObject.name);
        }
    }

    public override Sprite GetSprite()
    {
        return _desc.Sprite;
    }
    public override string GetName()
    {
        return _desc.Name;
    }
}
