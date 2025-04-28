using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Sheep : MonoBehaviour, IInteractable,IClickable,IDetectable, ISmashable
{
    [SerializeField] private PlayerSkill _currentInteract;
    public Vector3 PushedDirection;

    [SerializeField] float _speed;
    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorFeedback;

    private Vector3 _initialPosition;

    private bool _isGoodPosition = false;
    private bool _petCd = false;

    public Vector3 PositionToGo { get; set; }
    public bool CanGoTo { get; set; }
    public bool IsGoodPosition { get => _isGoodPosition; set => _isGoodPosition = value; }
    public bool NeedToFaceRight { get; set; }

    private Player _player;

    [SerializeField] AnimationClip _animationSmash;

    void Start()
    {
        PositionToGo = transform.GetChild(0).position;

        NeedToFaceRight = true;
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

                SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Mouton Fear One", "Mouton Fear Two", "Mouton Fear Three" },
                transform.position,0.8f);

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
            _animator.SetTrigger("Packed");
            StartCoroutine(GoUp());
            _currentInteract = PlayerSkill.BubbleSkill;
            SetPlayer();
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

    private IEnumerator GoDown()
    {
        float speedDown = _speed * 1.5625f; 
        float elapsedTime = 0f;

        while (elapsedTime < 0.32f)
        {
            transform.Translate(-transform.up * speedDown * Time.deltaTime);
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
            transform.Translate(PushedDirection.normalized * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            PositionToGo = transform.GetChild(0).position;
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
       if(_currentInteract == PlayerSkill.BubbleSkill && !IsGoodPosition)
        {
            CanGoTo = false;
            return;
        }

        CanGoTo = true;
    }

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        _player.StateMachine.ChangeState(_player.IdleState);
        
    }

    public void OnDestinationReached()
    {
        if (_currentInteract == PlayerSkill.None && IsGoodPosition == true)
            return;
        if (IsGoodPosition)
        {
            _animator.SetTrigger("Fall");
            StartCoroutine(GoDown());
            StartCoroutine(WaitOneSecond());

            _currentInteract = PlayerSkill.None;
            return;
        }

        if (AchievementSystem.Instance.PetCount < 10 && !_petCd)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            float animTime = (overrideController["A_Sheep01_Petting"].length - 0.01f) * 2;
            StartCoroutine(WaitForEndPetAnim(animTime));
        }

        _animator.SetTrigger("Pett");
        SetPlayer();
        StartCoroutine(WaitOneSecond());
    }

    private IEnumerator WaitForEndPetAnim(float animTime)
    {
        _petCd = true;

        yield return new WaitForSeconds(animTime);
        _petCd = false;
        AchievementSystem.Instance.PetCount++;
        AchievementSystem.Instance.PetAchievement();
    }

    public void Detected()
    {
        if (AngrySystem.Instance.IsAngry)
        {
            if(_animatorFeedback != null)
            {
                _animatorFeedback.SetTrigger("Detected");
            }
                    
            
            return;
        }
       
        
    }

    public void NoDetected()
    {
        if (_animatorFeedback != null)
        {
            _animatorFeedback.SetTrigger("NoDetected");
        }
    }

    public void SetAnimationSmash(Skill smashSkill)
    {
        smashSkill.AnimationSkill = _animationSmash;
    }
}
