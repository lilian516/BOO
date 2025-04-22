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

    public override void UseSkill()
    {
        Vector3 BubbleSpawnPos = _player.transform.position + _player.SkillDir.normalized;

        GameObject BubbleInstance = GameManager.Instance.SpawnObject(_desc.Prefab);

        float randomValue = Random.Range(0.8f, 1.2f);

        BubbleInstance.transform.position = BubbleSpawnPos;
        BubbleInstance.transform.localScale = Vector3.one * randomValue;
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
