using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerBee : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform[] _pathReferences;
    [SerializeField] AnimationClip _animationPiqure;
    private int _currentIndex;

    public float Speed;

    private float _timeStep;
    private bool _isResting;

    // Start is called before the first frame update
    void Start()
    {
        _currentIndex = 0;
        _timeStep = 0.0f;

        _isResting = false;

        transform.position = _pathReferences[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isResting)
        {
            WanderBetweenFlowers();

            CycleFlowers();
        }
    }

    private void WanderBetweenFlowers()
    {
        int DestinationIndex = _currentIndex == _pathReferences.Length - 1 ? 0 : _currentIndex + 1;

        Vector3 StartPos = _pathReferences[_currentIndex].position;
        Vector3 EndPos = _pathReferences[DestinationIndex].position;

        transform.position = Vector3.Lerp(StartPos, EndPos, _timeStep);

        _timeStep += Time.deltaTime * Speed;
    }

    private void CycleFlowers()
    {
        int DestinationIndex = _currentIndex == _pathReferences.Length - 1 ? 0 : _currentIndex + 1;

        if (transform.position == _pathReferences[DestinationIndex].position)
        {

            if (_pathReferences[(DestinationIndex + 1) % _pathReferences.Length].position.x < transform.position.x)
                GetComponent<SpriteRenderer>().flipX = false;
            else
                GetComponent<SpriteRenderer>().flipX = true;

            _currentIndex++;

            if (_currentIndex == _pathReferences.Length)
                _currentIndex = 0;

            _timeStep = 0.0f;

            StartCoroutine(Rest());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {


            if (!AngrySystem.Instance.IsAngry)
            {
                player.ChangeAnimAngry(_animationPiqure);
                player.StateMachine.ChangeState(player.AngryState);
            }


        }
    }

    private IEnumerator Rest()
    {
        _isResting = true;

        yield return new WaitForSeconds(2.0f);

        _isResting = false;
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill) {
            case PlayerSkill.PantsSkill:
                Destroy(gameObject); 
                break;
                
        }
    }
}
