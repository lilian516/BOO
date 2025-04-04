using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteractable, IChangeable
{
    private PlayerSkill _currentInteract;
    public Vector3 PushedDirection;

    [SerializeField] float _speed;
    [SerializeField] Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentInteract = PlayerSkill.None;

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
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


    public void Change()
    {
        transform.Rotate(Vector3.right * 180);
    }

    public void ResetChange()
    {
        transform.Rotate(Vector3.right * -180);

    }

    private IEnumerator Pushed()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.Translate(PushedDirection * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
}
