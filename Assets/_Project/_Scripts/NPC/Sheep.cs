using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteractable
{
    private PlayerSkill _currentInteract;

    [SerializeField] float _speed;
    [SerializeField] Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentInteract = PlayerSkill.None;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.BubbleSkill:
               
                PackedSheep();
                break;
        }
    }

    private void PackedSheep()
    {
        if(_currentInteract != PlayerSkill.BubbleSkill)
        {
            _animator.SetTrigger("Packed");
            StartCoroutine(GoUp());
            _currentInteract = PlayerSkill.BubbleSkill;
            
            Debug.Log("je suis embullé !!");
        }
        
    }

    private IEnumerator GoUp()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            
            transform.Translate(transform.up * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
    }
}
