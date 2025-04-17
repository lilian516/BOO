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
        OnExitUseSkill?.Invoke();
    }

    public void UseSkillState()
    {
        Debug.Log("oui utilise");
        OnEnterUseSkill?.Invoke();
    }

    public void ExitAngryStateAnim()
    {
        Debug.Log("oui fin");
        OnExitAngryState?.Invoke();
    }

    public void SoundWalkBoo()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Grass Step One", "Grass Step Two", "Grass Step Three", "Grass Step Four", "Grass Step Five" }, transform.position);
    }
}
