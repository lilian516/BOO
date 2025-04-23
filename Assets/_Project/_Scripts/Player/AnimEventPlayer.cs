using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Grass Step One", "Grass Step Two", "Grass Step Three", "Grass Step Four", "Grass Step Five" }, transform.position);
    }
    public void SheepVFX()
    {
        GameObject VFX = Instantiate(GameManager.Instance.Player.GetComponent<Player>().KillSheepVFX, GameManager.Instance.Player.transform.position + new Vector3(0.0f, 0.15f, 0.0f), GameManager.Instance.Player.GetComponent<Player>().KillSheepVFX.transform.rotation);
        SceneManager.MoveGameObjectToScene(VFX, SceneManager.GetSceneByName("MainScene"));
        ParticleSystem parts = VFX.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration;
        Destroy(VFX, totalDuration);
    }
}
