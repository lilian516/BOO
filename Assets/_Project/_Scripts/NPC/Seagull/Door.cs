using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IClickable
{

    [SerializeField] Animator _animator;

    public bool IsKnocked = false;

    
    
    public void OnClick()
    {
        _animator.SetTrigger("Vomit");
        IsKnocked = true;
    }

    
}
