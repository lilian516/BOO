using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerSkill _currentInteract;
    public Vector3 PushedDirection;

    [SerializeField] float _speed;
    [SerializeField] Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

            case PlayerSkill.WindSkill:
                MoveSheep();
                break;

            case PlayerSkill.SmashSkill:
                AngrySystem.Instance.ChangeCalmLimits();

                GameManager.Instance.KilledSheep++;
                Destroy(this.gameObject);
                break;
        }
    }

    private void MoveSheep()
    {
        if (_currentInteract == PlayerSkill.BubbleSkill)
        {
            StartCoroutine(Pushed());
        }
    }

    private void PackedSheep()
    {
        if(_currentInteract != PlayerSkill.BubbleSkill)
        {
            _animator.SetTrigger("Packed");
            StartCoroutine(GoUp());
            _currentInteract = PlayerSkill.BubbleSkill;
            
            Debug.Log("je suis embull� !!");
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


    private IEnumerator Pushed()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            transform.Translate(PushedDirection * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
}
