using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
   
    public void EndVomit()
    {

        //Debug.Log("je suis en test");
        
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        //Debug.Log("je suis énervé");
        player.StateMachine.ChangeState(player.AngryState);
    }
}
