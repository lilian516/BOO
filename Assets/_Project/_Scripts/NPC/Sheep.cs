using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteractable,IClickable
{
    [SerializeField] private PlayerSkill _currentInteract;
    public Vector3 PushedDirection;

    [SerializeField] float _speed;
    [SerializeField] Animator _animator;

    private Vector3 _initialPosition;

    public Vector3 PositionToGo { get; set; }

    private Player _player;

    void Start()
    {
        PositionToGo = transform.GetChild(0).position;
        
    }

    private void SetPlayer()
    {
        if(_player == null)
        {
            _player = GameManager.Instance.Player.GetComponent<Player>();
        }
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
                Destroy(gameObject);
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
            Debug.Log("je pack");
            _animator.SetTrigger("Packed");
            StartCoroutine(GoUp());
            _currentInteract = PlayerSkill.BubbleSkill;
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

        _initialPosition = transform.position;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            StartCoroutine(LetSheepFloat());
        }
    }

    private IEnumerator LetSheepFloat()
    {
        yield return new WaitForSeconds(3.0f);

        float ElapsedTime = 0.0f;

        while (ElapsedTime < 3.0f)
        {
            Vector3 MoveDirection = _initialPosition - transform.position;
            transform.Translate(MoveDirection * _speed * Time.deltaTime);
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void OnClick()
    {
        //Debug.Log("on caresse");
        _animator.SetTrigger("Pett");
        SetPlayer();
        StartCoroutine(WaitOneSecond());
    }

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        _player.StateMachine.ChangeState(_player.IdleState);
    }
}
