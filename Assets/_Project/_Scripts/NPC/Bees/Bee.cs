using System.Collections;
using UnityEngine;

public class Bee : MonoBehaviour, IInteractable, IChangeable, ISmashable
{
    [SerializeField] float _speed;
    [SerializeField] Animator _animator;
    [SerializeField] BeeAnimEventPlayer _eventPlayer;
    [SerializeField] AnimationClip _animationPiqure;
    [SerializeField] AnimationClip _animationSmash;

    private BeeState _currentState;

    private bool _isBubbled;

    [Header("Patrol State")]
    [SerializeField] private Transform[] _pathReferences;
    [SerializeField] float _patrolDistance;
    private int _currentIndex;
    private Vector3 _currentPosition;
    private float _timeStep;
    private bool _isResting;

    [Header("Attack State")]
    [SerializeField] float _attackDistance;
    [SerializeField] BeeTriggerZone _triggerZone;
    private bool _onCd;

    void Start()
    {
        _isResting = false;
        _currentPosition = transform.position;
        _currentIndex = 0;
        _timeStep = 0.0f;
        _currentState = BeeState.Idle;
        _eventPlayer.OnEndAttackAnim += Attack;

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }
    void Update()
    {
        if (_isBubbled)
            return;

        GameObject player;
        if (!(player = GameManager.Instance.Player))
            return;

        switch (_currentState)
        {
            case BeeState.Idle:
                if(Vector3.Distance(player.transform.position, transform.position) <= _patrolDistance)
                {
                    _currentState = BeeState.Patrol;
                }
                
                break;
            case BeeState.Patrol:
                if (!_isResting)
                {
                    GoToPoint(_pathReferences[_currentIndex].position,_speed / 2);

                    CyclePoints();
                }

                if (Vector3.Distance(player.transform.position, transform.position) > _patrolDistance)
                {
                    _currentState = BeeState.Idle;
                    _currentPosition = transform.position;
                    _timeStep = 0.0f;
                }
                else if (_triggerZone.IsTrigger)
                {
                    _currentPosition = transform.position;
                    _timeStep = 0.0f;
                    _currentState = BeeState.Attack;
                    player.GetComponent<Player>().StateMachine.ChangeState(player.GetComponent<Player>().WaitingState);

                    if (transform.position.x > player.transform.position.x)
                        GetComponentInChildren<SpriteRenderer>().flipX = false;
                    else
                        GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
                
                break;
            case BeeState.Attack:
                if (!_onCd)
                {

                    if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) <= 1f)
                    {

                        _currentPosition = transform.position;
                        _timeStep = 0.0f;
                        _animator.SetTrigger("Attack");
                        StartCoroutine(AttackCD());
                    }
                    else
                        GoToPoint(player.transform.position, _speed);
                }
                if (Vector3.Distance(player.transform.position, transform.position) <= _patrolDistance && !_triggerZone.IsTrigger)
                {
                    if (GetComponentInChildren<SpriteRenderer>().flipX)
                        GetComponentInChildren<SpriteRenderer>().flipX = false;

                    _currentState = BeeState.Patrol;
                    _timeStep = 0.0f;
                }
                else if (Vector3.Distance(player.transform.position, transform.position) > _patrolDistance && !_triggerZone.IsTrigger)
                {
                    if (GetComponentInChildren<SpriteRenderer>().flipX)
                        GetComponentInChildren<SpriteRenderer>().flipX = false;

                    _currentState = BeeState.Idle;
                    _timeStep = 0.0f;
                }
                


                    break;

            case BeeState.Fear:
                
                
                    
                
                break;
            default:
                break;
        }
    }
    private void GoToPoint(Vector3 Dest, float speed)
    {
        transform.position = Vector3.Lerp(_currentPosition, Dest, _timeStep);
        _timeStep += Time.deltaTime * speed;
    }
    private void CyclePoints()
    {

        if (transform.position == _pathReferences[_currentIndex].position)
        {
            _currentPosition = transform.position;
            if (_pathReferences[(_currentIndex + 1) % _pathReferences.Length].position.x < transform.position.x)
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            else
                GetComponentInChildren<SpriteRenderer>().flipX = true;

            _currentIndex++;

            if (_currentIndex == _pathReferences.Length)
                _currentIndex = 0;

            _timeStep = 0.0f;

            StartCoroutine(Rest());
        }
    }
    private void Attack()
    {
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        player.RB.AddForce(-transform.forward * 200);

        if (!AngrySystem.Instance.IsAngry)
            player.ChangeAnimAngry(_animationPiqure);
        else
        {
            player.StateMachine.ChangeState(player.WaitingState);
            StartCoroutine(PlayerStun());
            return;
        }

        player.StateMachine.ChangeState(player.AngryState);
    }

    private IEnumerator PlayerStun()
    {
        yield return new WaitForSeconds(0.8f);
        GameManager.Instance.Player.GetComponent<Player>().StateMachine.ChangeState(GameManager.Instance.Player.GetComponent<Player>().IdleState);
    }

    private IEnumerator AttackCD()
    {
        _onCd = true;
        yield return new WaitForSeconds(2);
        _onCd = false;
    }
    private IEnumerator Rest()
    {
        _isResting = true;

        yield return new WaitForSeconds(1f);

        _isResting = false;
    }
    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.BubbleSkill:
                _triggerZone.gameObject.SetActive(false);
                _isBubbled = true;
                PackedBee();
                break;
            case PlayerSkill.SmashSkill:
                AngrySystem.Instance.ChangeCalmLimits();
                Destroy(gameObject);
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

        while (elapsedTime < 0.25f)
        {
            transform.Translate(transform.up * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    

    public void Change()
    {
        _animator.SetTrigger("Fear");
        _currentState = BeeState.Fear;
    }

    public void ResetChange()
    {
        _currentState = BeeState.Idle;
        _animator.SetTrigger("Idle");
    }

    private void OnDestroy()
    {
        _eventPlayer.OnEndAttackAnim -= Attack;

        if(AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements -= Change;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
        
    }

    public void SetAnimationSmash(Skill smashSkill)
    {
        smashSkill.AnimationSkill = _animationSmash;
    }

    private enum BeeState
    {
        Idle,
        Patrol,
        Attack,
        Fear
    }
}
