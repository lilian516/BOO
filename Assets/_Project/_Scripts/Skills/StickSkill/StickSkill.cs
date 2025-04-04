using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickSkill : Skill
{

    [System.Serializable]
    public class Descriptor
    {
        public float Radius;
        public LayerMask Mask;
        public Sprite Sprite;
        public string Name;
    }

    Descriptor _desc;
    public StickSkill(Player player, Descriptor desc) : base(player)
    {
        _desc = desc;
    }


    public override void UseSkill()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _desc.Radius, _desc.Mask);

        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact(PlayerSkill.StickSkill);
                
            }
            Debug.Log("Objet détecté : " + hitCollider.gameObject.name);
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
