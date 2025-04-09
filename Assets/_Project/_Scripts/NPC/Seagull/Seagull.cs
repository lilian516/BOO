using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{

    [SerializeField] Animator _animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            // Ajouter l'animation de chier sur Boo
            _animator.SetTrigger("Vomit");
            AngrySystem.Instance.ChangeAngryLimits();
        }
    }
}
