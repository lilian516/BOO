using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class JoystickUI : MonoBehaviour
{

    private Vector3 _centerPos;
    private GameObject _middleCircle;
    private GameObject _joystick;
    void Start()
    {
        _centerPos = transform.GetChild(0).position;
        _middleCircle = transform.GetChild(1).gameObject;
        _joystick = transform.GetChild(2).gameObject;
    }

    void Update()
    {
        if (InputManager.Instance.GetMoveDirection() != Vector2.zero)
        {
            _middleCircle.transform.position = _centerPos + (_joystick.transform.position - _centerPos) / 3;
        }
    }
}
