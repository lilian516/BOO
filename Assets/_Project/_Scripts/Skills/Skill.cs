using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    protected Player _player;

    public AnimationClip AnimationSkill;
    public Skill(Player player)
    {
        _player = player;
    }
    

    public virtual void UseSkill()
    {
        //Debug.Log("j'utilise mon skill");
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }

    public virtual string GetName()
    {
        return null;
    }
}
