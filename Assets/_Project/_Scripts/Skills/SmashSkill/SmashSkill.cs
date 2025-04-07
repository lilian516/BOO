using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SmashSkill : Skill
{

    [System.Serializable]
    public class Descriptor
    {
        public float Radius;
        public Sprite Sprite;
        public LayerMask Mask;
    }

    Descriptor _desc;

    public SmashSkill(Player player, Descriptor desc) : base(player)
    {
        _desc = desc;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void UseSkill()
    {
        Debug.Log("TAPER");
        Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _desc.Radius, _desc.Mask);

        if (hitColliders.Length == 0)
            return;

        IInteractable interactable = hitColliders[0].gameObject.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(PlayerSkill.SmashSkill);

        }
        Debug.Log("Objet TAPER : " + hitColliders[0].gameObject.name);
    }

    public override Sprite GetSprite()
    {
        return _desc.Sprite;
    }
}
