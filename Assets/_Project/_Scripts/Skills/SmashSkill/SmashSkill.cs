using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmashSkill : Skill
{

    public SmashSkill(Player player, SkillDescriptor desc) : base(player)
    {
        _desc = desc;
    }


    public override void UseSkill()
    {
        //Debug.Log("TAPER");
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _desc.Radius, _desc.Mask);
        Debug.Log(hitColliders.Length);
        if (hitColliders.Length == 0)
            return;

        int ClosestColliderIndex = 0;
        float ClosestColliderLength = 10000.0f;
        float CurrentColliderDistance;

        foreach (Collider collider in hitColliders)
        {
            Vector3 ColliderPlayerDistance = collider.transform.position - GameManager.Instance.Player.transform.position;

            CurrentColliderDistance = ColliderPlayerDistance.magnitude;

            if (CurrentColliderDistance < ClosestColliderLength)
            {
                ClosestColliderLength = CurrentColliderDistance;
                ClosestColliderIndex = System.Array.IndexOf(hitColliders, collider);
            }
        }

        IInteractable interactable = hitColliders[ClosestColliderIndex].gameObject.GetComponent<IInteractable>();
        
        if (interactable != null)
        {
            interactable.Interact(PlayerSkill.SmashSkill);

        }
        //Debug.Log("Objet TAPER : " + hitColliders[0].gameObject.name);
    }

    public override Sprite GetSprite()
    {
        return _desc.Sprite;
    }
}
