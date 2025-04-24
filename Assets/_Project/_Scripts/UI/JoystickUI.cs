using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class JoystickUI : MonoBehaviour, IChangeable
{

    private Vector3 _centerPos;
    private GameObject _middleCircle;
    private GameObject _joystick;
    private float _baseRange;

    void Start()
    {
        _centerPos = transform.GetChild(0).position;
        _middleCircle = transform.GetChild(1).gameObject;
        _joystick = transform.GetChild(2).gameObject;
        _baseRange = _joystick.GetComponent<OnScreenStick>().movementRange;

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    void Update()
    {
        if (InputManager.Instance.GetMoveDirection() != Vector2.zero)
        {
            _middleCircle.transform.position = _centerPos + (_joystick.transform.position - _centerPos) / 3;
        }
    }
    public void Change()
    {
        StartCoroutine(BlockControl());
    }

    public void ResetChange()
    {
        StartCoroutine(BlockControl());
    }

    private IEnumerator BlockControl()
    {
        _joystick.GetComponent<OnScreenStick>().movementRange = 0;
        _joystick.transform.position = _centerPos;
        _middleCircle.transform.position = _centerPos;

        yield return new WaitForSeconds(3.7f);

        _joystick.GetComponent<OnScreenStick>().movementRange = _baseRange;
    }
}

