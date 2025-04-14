using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventPlayer : MonoBehaviour
{

    public bool IsExitUseSkill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitUseSkillState()
    {
        Debug.Log("oui on sort");
        IsExitUseSkill = true;
    }

    public void SoundWalkBoo()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Grass Step One", "Grass Step Two", "Grass Step Three", "Grass Step Four", "Grass Step Five" }, transform.position);
    }
}
