using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWalking : MonoBehaviour
{
    public Player Player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        Player.GetComponent<Player>().CanWalkForward = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        Player.GetComponent<Player>().CanWalkForward = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        Player.GetComponent<Player>().CanWalkForward = true;
    }
}
