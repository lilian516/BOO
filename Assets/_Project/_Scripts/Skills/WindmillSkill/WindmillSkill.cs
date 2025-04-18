using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSkill : Skill
{
    public WindmillSkill(Player player, SkillDescriptor desc) : base(player)
    {
        _desc = desc;
        AnimationSkill = _desc.AnimationSkill;
    }

    public override void UseSkill()
    {
        Vector3 spawnPos = _player.transform.position + _player.SkillDir.normalized;

        GameObject wind = GameManager.Instance.SpawnObject(_desc.Prefab);

        wind.GetComponent<Wind>().Init(_player.SkillDir, _player.transform.GetChild(3).rotation);

        wind.transform.position = spawnPos;
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
