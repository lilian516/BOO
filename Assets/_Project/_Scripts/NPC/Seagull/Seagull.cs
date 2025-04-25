using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Seagull : MonoBehaviour, IChangeable
{

    
    [SerializeField] Animator _animatorBigSeagull;
    [SerializeField] AnimationClip _animationVomit;
    public void Change()
    {
        _animatorBigSeagull.SetBool("IsFear", true);
    }

    private void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    public void EndVomit()
    {
        
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        player.ChangeAnimAngry(_animationVomit);
        player.StateMachine.ChangeState(player.AngryState);
    }

    public void SoundVomit()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Vomit One", "Vomit Two", "Vomit Three" },
            transform.position);
    }

    public void ResetChange()
    {
        _animatorBigSeagull.SetBool("IsFear", false);
    }
}
