using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmashSkill : Skill
{
    private IInteractable _interactable;
    public SmashSkill(Player player, SkillDescriptor desc) : base(player)
    {
        _desc = desc;
    }


    public override void UseSkill()
    {
       
        if (_interactable != null)
        {
            _interactable.Interact(PlayerSkill.SmashSkill);

        }
    }

    public override Sprite GetSprite()
    {
        return _desc.Sprite;
    }

    public override void StartUseSkill()
    {
        base.StartUseSkill();
        _interactable = null;

        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _desc.Radius, _desc.Mask);

        if (hitColliders.Length == 0)
        {
            AnimationSkill = _desc.AnimationSkill;
            return;
        }
            

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
            _interactable = hitColliders[ClosestColliderIndex].gameObject.GetComponent<IInteractable>();
            Sheep sheep = hitColliders[ClosestColliderIndex].gameObject.GetComponent<Sheep>();
            if (sheep != null)
            {
                AnimationSkill = sheep.AnimationSmash;
            }
            else
            {
                AnimationSkill = _desc.AnimationSkill;
            }
        }

        
    }

    
}
