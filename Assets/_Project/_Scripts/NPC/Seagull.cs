using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            // Ajouter l'animation de chier sur Boo

            AngrySystem.Instance.ChangeAngryLimits();
        }
    }
}
