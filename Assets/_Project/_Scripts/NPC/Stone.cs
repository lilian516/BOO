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

    // Start is called before the first frame update
    void Start()
    {
        _currentIndex = 0;
        _timeStep = 0.0f;

        _isStopped = true;

        transform.position = _pathPoints[0].PathReference.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isStopped)
            return;

        GoToPathPoint();

        CyclePathPoints();
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.StickSkill:
                _isStopped = false;
                Debug.Log("BOUGE TA MERE DE LA");
                break;
        }
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
        int DestinationIndex = _currentIndex == _pathPoints.Length - 1 ? 0 : _currentIndex + 1;

        Vector3 StartPos = _pathPoints[_currentIndex].PathReference.position;
        Vector3 EndPos = _pathPoints[DestinationIndex].PathReference.position;

        transform.position = Vector3.Lerp(StartPos, EndPos, _timeStep);

        _timeStep += Time.deltaTime * Speed;
    }

    private void CyclePathPoints()
    {
        int DestinationIndex = _currentIndex == _pathPoints.Length - 1 ? 0 : _currentIndex + 1;

        if (transform.position == _pathPoints[DestinationIndex].PathReference.position)
        {

            if (_pathPoints[(DestinationIndex + 1) % _pathPoints.Length].PathReference.position.x < transform.position.x)
                transform.eulerAngles = new Vector3(30, 0, 0);
            else
                transform.eulerAngles = new Vector3(-30, 180, 0);

            _currentIndex++;

            if (_pathPoints[_currentIndex].IsStop || _currentIndex + 1 == _pathPoints.Length)
                _isStopped = true;

            _timeStep = 0.0f;
        }
    }
}
