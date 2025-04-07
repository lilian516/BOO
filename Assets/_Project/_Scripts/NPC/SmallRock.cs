using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            // Ajouter l'animation de Boo qui trebuche

            Debug.Log("TOMBER");
            AngrySystem.Instance.ChangeAngryLimits();
        }
    }
}
