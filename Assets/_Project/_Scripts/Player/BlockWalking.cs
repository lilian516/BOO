using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWalking : MonoBehaviour
{
    [SerializeField] private Player _player;
    private bool _alreadyValid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        if (!_alreadyValid)
        {
            _player.ValidCapsuleDetector++;
            _alreadyValid = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        _player.RB.velocity = Vector3.zero;

        if (_alreadyValid)
        {
            _player.ValidCapsuleDetector--;
            _alreadyValid = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 3)
            return;

        if (!_alreadyValid)
        {
            _player.ValidCapsuleDetector++;
            _alreadyValid = true;
        }
    }
}
