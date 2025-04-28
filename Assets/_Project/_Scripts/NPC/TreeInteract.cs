using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : MonoBehaviour, IInteractable
{
    private Animator _animator;
    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill) {
            case PlayerSkill.PantsSkill:
                Debug.Log("test");
                _animator.SetTrigger("Touched");
                break;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
