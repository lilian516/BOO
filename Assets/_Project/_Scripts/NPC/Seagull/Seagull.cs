using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Seagull : MonoBehaviour, IChangeable
{

    
    [SerializeField] Animator _animatorBigSeagull;
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
        player.StateMachine.ChangeState(player.AngryState);
    }

    public void ResetChange()
    {
        _animatorBigSeagull.SetBool("IsFear", false);
    }
}
