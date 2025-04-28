using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _fans;
    private float _currentRotationStrength;
    private float _currentRotationSpeed;
    private float _oldRotationSpeed;
    private float _newRotationSpeed;
    private float _timeStep;

    private void Start()
    {
        _currentRotationStrength = 1.0f;
        _currentRotationSpeed = 1.0f;
        _oldRotationSpeed = _currentRotationSpeed;
        _newRotationSpeed = _currentRotationSpeed;

        _timeStep = 0.0f;

        StartCoroutine(ChangeFanSpeed(5.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentRotationStrength < 1.0f)
        {
            _currentRotationStrength += Time.deltaTime;
            foreach (GameObject fan in _fans)
            {
                fan.GetComponent<Fan>().RotationStrength = _currentRotationStrength;
            }
        }

        _currentRotationSpeed = Mathf.Lerp(_oldRotationSpeed, _newRotationSpeed, _timeStep);
        foreach (GameObject fan in _fans)
        {
            fan.GetComponent<Fan>().RotationSpeed = _currentRotationSpeed;
        }

        _timeStep += Time.deltaTime;
    }

    private IEnumerator ChangeFanSpeed(float Time)
    {
        yield return new WaitForSeconds(Time);

        _oldRotationSpeed = _currentRotationSpeed;
        _newRotationSpeed = Random.Range(1.0f, 5.0f);
        _timeStep = 0.0f;

        StartCoroutine(ChangeFanSpeed(Random.Range(13.0f, 22.0f)));
    }
}
