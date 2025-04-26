using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWalking : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        _player.CanWalkForward = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        _player.RB.velocity = Vector3.zero;
        _player.CanWalkForward = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        _player.CanWalkForward = true;
    }
}
