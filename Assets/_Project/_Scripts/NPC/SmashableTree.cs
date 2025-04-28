using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashableTree : MonoBehaviour, IInteractable
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.SmashSkill:
                Debug.Log("je tape");
                _animator.SetTrigger("Fall");
                break;
        }
    }



}
