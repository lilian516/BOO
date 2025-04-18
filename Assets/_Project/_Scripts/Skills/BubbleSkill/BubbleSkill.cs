using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class BubbleSkill : Skill
{
    public BubbleSkill(Player player, SkillDescriptor desc) : base(player)
    {
        _desc = desc;
        AnimationSkill = _desc.AnimationSkill;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void UseSkill()
    {
        /* Cette fonction devra remplacer la création manuelle d'un plane et d'ajouter sa texture 
         * par l'instanciation d'un GameObject Type "Bubble" possédant les propriétés correspondantes.
         * De même, la position de spawn devra être changée et la direction donnée à la bulle ajoutée 
         * donnant le forward vector du joueur.
        */

        Debug.Log("j'utilise mon skill bubble");
        Vector3 BubbleSpawnPos = _player.transform.position + _player.SkillDir.normalized;

        //Instantiate(BubblePrefab);

        GameObject BubbleInstance = GameManager.Instance.SpawnObject(_desc.Prefab);

        //GameObject BubblePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        float randomValue = Random.Range(0.8f, 1.2f);

        BubbleInstance.transform.position = BubbleSpawnPos;
        BubbleInstance.transform.localScale = Vector3.one * randomValue;

        //BubblePlane.GetComponent<Renderer>().material = BubbleMaterial;
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
