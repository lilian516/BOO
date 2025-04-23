using System.Collections;
using UnityEngine;

public class Bee : MonoBehaviour, IInteractable
{
    [SerializeField] float _speed;
    [SerializeField] Animator _animator;
    [SerializeField] BeeAnimEventPlayer _eventPlayer;

    private BeeState _currentState;

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
    }
    void Update()
    {
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
                    GoToPoint(_pathReferences[_currentIndex].position,_speed / 7);

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
                }
                break;
            case BeeState.Attack:
                if (!_onCd)
                {

                    if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) <= 0.8f)
                    {
                        if (transform.position.x > player.transform.position.x)
                            GetComponentInChildren<SpriteRenderer>().flipX = false;
                        else
                            GetComponentInChildren<SpriteRenderer>().flipX = true;

                        _currentPosition = transform.position;
                        _timeStep = 0.0f;
                        _animator.SetTrigger("Attack");
                        StartCoroutine(AttackCD());
                    }
                    else
                        GoToPoint(player.transform.position, _speed / 2);
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
        AngrySystem.Instance.ChangeAngryLimits();
        StartCoroutine(PlayerStun());
        
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

    private enum BeeState
    {
        Idle,
        Patrol,
        Attack
    }
}
