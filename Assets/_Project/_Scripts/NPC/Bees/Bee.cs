using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour, IInteractable
{
    [SerializeField] float _speed;
    [SerializeField] Animator _animator;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.BubbleSkill:

                PackedBee();
                break;
        }
    }

    private void PackedBee()
    {
        _animator.SetTrigger("Packed");
        StartCoroutine(GoUp());

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
