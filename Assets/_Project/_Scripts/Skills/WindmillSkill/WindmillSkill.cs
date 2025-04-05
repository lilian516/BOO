using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSkill : Skill
{

    [System.Serializable]
    public class Descriptor
    {
        public GameObject WindPrefab;
        public Sprite Sprite;
        public string Name;
    }

    Descriptor _desc;

    public WindmillSkill(Player player, Descriptor desc) : base(player)
    {
        _desc = desc;
    }

    public override void UseSkill()
    {
        Vector3 spawnPos = _player.transform.position;

        GameObject wind = GameManager.Instance.SpawnObject(_desc.WindPrefab);

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
