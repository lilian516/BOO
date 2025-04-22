using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventPlayer : MonoBehaviour
{
    public delegate void ExitUseSkill();
    public ExitUseSkill OnExitUseSkill;
    public delegate void EnterUseSkill();
    public EnterUseSkill OnEnterUseSkill;
    public delegate void ExitAngryState();
    public ExitAngryState OnExitAngryState;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ExitUseSkillState()
    {
        OnExitUseSkill?.Invoke();
    }

    public void UseSkillState()
    {
        OnEnterUseSkill?.Invoke();
    }

    public void ExitAngryStateAnim()
    {
        OnExitAngryState?.Invoke();
    }

    public void ExitTakeState()
    {
        OnExitUseSkill?.Invoke();
    }

    public void SoundWalkBoo()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Grass Step One", "Grass Step Two", "Grass Step Three", "Grass Step Four", "Grass Step Five" }, transform.position);
    }
}
