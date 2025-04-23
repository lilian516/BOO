using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeTriggerZone : MonoBehaviour
{
    public bool IsTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.Player)
            IsTrigger = true;
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject == GameManager.Instance.Player)
            IsTrigger = false;
    }
}
