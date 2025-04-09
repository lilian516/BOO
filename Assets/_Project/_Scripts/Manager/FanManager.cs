using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _fans;
    private bool _isRotatingClockwise;
    private float _currentRotationStrength;
    private float _currentRotationSpeed;
    private float _oldRotationSpeed;
    private float _newRotationSpeed;
    private float _timeStep;
    void Start()
    {
        _isRotatingClockwise = true;
        _currentRotationStrength = 1.0f;
        _currentRotationSpeed = 1.0f;
        _oldRotationSpeed = _currentRotationSpeed;
        _newRotationSpeed = _currentRotationSpeed;

        _timeStep = 0.0f;

        StartCoroutine(ChangeFanSpeed(5.0f));
        StartCoroutine(ChangeFanRotation(Random.Range(10.0f, 20.0f)));
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRotatingClockwise && _currentRotationStrength < 1.0f)
        {
            _currentRotationStrength += Time.deltaTime;
            foreach (GameObject fan in _fans)
            {
                fan.GetComponent<Fan>().RotationStrength = _currentRotationStrength;
            }
        }
        else if (!_isRotatingClockwise && _currentRotationStrength > -1.0f)
        {
            _currentRotationStrength -= Time.deltaTime;
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

    private IEnumerator ChangeFanRotation(float Time)
    {
        yield return new WaitForSeconds(Time);

        _isRotatingClockwise = !_isRotatingClockwise;

        StartCoroutine(ChangeFanRotation(Random.Range(13.0f, 22.0f)));
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
