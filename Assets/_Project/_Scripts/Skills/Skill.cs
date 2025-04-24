using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    protected Player _player;

    protected SkillDescriptor _desc;

    public AnimationClip AnimationSkill;
    public Skill(Player player)
    {
        _player = player;
    }
    
    public virtual void StartUseSkill()
    {

    }
    public virtual void UseSkill()
    {
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
