using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IInteractable
{
    [Serializable]
    struct PathPoint 
    {
        public Transform PathReference;
        public bool IsStop;
    }

    [SerializeField] private PathPoint[] _pathPoints;
    public float Speed = 2.0f;

    private bool _isStopped;
    private int _currentIndex;
    private float _timeStep;

    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _currentIndex = 0;
        _timeStep = 0.0f;

        _isStopped = true;

        transform.position = _pathPoints[0].PathReference.position - new Vector3(0, 0.3f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStopped || _currentIndex >= _pathPoints.Length)
            return;

        GoToPathPoint();

        CyclePathPoints();
    }

    public void Interact(PlayerSkill playerSkill)
    {
        if (_currentIndex + 1 >= _pathPoints.Length)
            _currentIndex = 0;

        switch (playerSkill)
        {
            case PlayerSkill.StickSkill:
                _isStopped = false;
                _animator.SetTrigger("Return");
                transform.position += new Vector3(0, 0.3f, 0);
                break;
        }

        if (_pathPoints[_currentIndex + 1].PathReference.position.x > _pathPoints[_currentIndex].PathReference.position.x && transform.localScale.z > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
        }
        else if (transform.localScale.z < 0)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && _isStopped)
        {
            // Ajouter l'animation de Boo qui trébuche

            AngrySystem.Instance.ChangeAngryLimits();
        }
    }

    private void GoToPathPoint()
    {
        int DestinationIndex = (_currentIndex + 1)% _pathPoints.Length;

        Vector3 StartPos = _pathPoints[_currentIndex % _pathPoints.Length].PathReference.position;
        Vector3 EndPos = _pathPoints[DestinationIndex].PathReference.position;

        transform.position = Vector3.Lerp(StartPos, EndPos, _timeStep);

        _timeStep += Time.deltaTime * (Speed * Vector3.Distance(StartPos,EndPos)) / 3;
    }

    private void CyclePathPoints()
    {
        int DestinationIndex = (_currentIndex + 1) % _pathPoints.Length;

        if (transform.position == _pathPoints[DestinationIndex].PathReference.position)
        {

            _currentIndex++;

            if (_pathPoints[_currentIndex % _pathPoints.Length].IsStop || _currentIndex + 1 >= _pathPoints.Length)
            {
                _isStopped = true;
                transform.position += new Vector3(0, -0.3f, 0);
                if (transform.localScale.z < 0)
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
            }
                
            _timeStep = 0.0f;

            if (_pathPoints[DestinationIndex].PathReference.position.x > _pathPoints[DestinationIndex - 1].PathReference.position.x && transform.localScale.z > 0)
                 transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * - 1);
            else if (transform.localScale.z < 0)
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * -1);
        }
    }
}
