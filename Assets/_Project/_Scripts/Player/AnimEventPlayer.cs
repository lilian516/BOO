using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Grass One", "Grass Two", "Grass Three", "Grass Four", "Grass Five" }, transform.position);
    }
    public void SheepVFX()
    {
        GameObject VFX = Instantiate(GameManager.Instance.Player.GetComponent<Player>().KillSheepVFX, GameManager.Instance.Player.transform.position + new Vector3(0.0f, 0.15f, 0.0f), GameManager.Instance.Player.GetComponent<Player>().KillSheepVFX.transform.rotation);
        SceneManager.MoveGameObjectToScene(VFX, SceneManager.GetSceneByName("MainScene"));
        ParticleSystem parts = VFX.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration;
        Destroy(VFX, totalDuration);
    }

    public void SoundBooBlow()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Blow One", "Boo Blow Two", "Boo Blow Three", "Boo Blow Four",
            "Boo Blow Five","Boo Blow Six","Boo Blow Seven" }, 
            transform.position);
    }

    public void SoundBooDamage()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Damage One", "Boo Damage Two", "Boo Damage Three", "Boo Damage Four",
            },
            transform.position);
    }

    public void SoundBooFall()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Fall One", "Boo Fall Two",
            },
            transform.position);
    }

    public void SoundBooSlip()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Slip One", "Boo Slip Two","Boo Slip Three","Boo Slip Four"
            },
            transform.position);
    }

    public void SoundBooSlipVoice()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Fall One", "Boo Fall Two",
            },
            transform.position);
    }

    public void SoundBooStick()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Stick One", "Boo Stick Two",
            },
            transform.position);
    }
}
