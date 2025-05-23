using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour, IInteractable, ISmashable
{

    [SerializeField] private Transform[] _pathReferences;

    [SerializeField] AnimationClip _animationSmash;
    private int _currentIndex;

    public float Speed;

    private float _timeStep;
   


    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.SmashSkill:
                AngrySystem.Instance.ChangeCalmLimits();

                Destroy(gameObject);
                break;
        }

    }
    void Start()
    {
        _currentIndex = 0;
        _timeStep = 0.0f;
        transform.position = _pathReferences[0].position;
    }

    void Update()
    {
        WanderBetweenPositions();
        CycleMoving();
    }


    private void WanderBetweenPositions()
    {
        int DestinationIndex = _currentIndex == _pathReferences.Length - 1 ? 0 : _currentIndex + 1;
        Vector3 StartPos = _pathReferences[_currentIndex].position;
        Vector3 EndPos = _pathReferences[DestinationIndex].position;

        transform.position = Vector3.Lerp(StartPos, EndPos, _timeStep);

        _timeStep += Time.deltaTime * Speed;
    }

    private void CycleMoving()
    {
        int DestinationIndex = _currentIndex == _pathReferences.Length - 1 ? 0 : _currentIndex + 1;

        if (transform.position == _pathReferences[DestinationIndex].position)
        {

            if (_pathReferences[(DestinationIndex + 1) % _pathReferences.Length].position.x < transform.position.x)
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            else
                GetComponentInChildren<SpriteRenderer>().flipX = true;

            _currentIndex++;

            if (_currentIndex == _pathReferences.Length)
                _currentIndex = 0;


            _timeStep = 0.0f;

        }
    }

    public void SetAnimationSmash(Skill smashSkill)
    {
        smashSkill.AnimationSkill = _animationSmash;
        
    }
}
