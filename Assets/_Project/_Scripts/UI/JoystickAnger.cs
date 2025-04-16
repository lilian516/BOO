using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAnger : MonoBehaviour
{
    [SerializeField] List<Sprite> _spriteList;
    [SerializeField] Image _joystickImage;
    private bool _isBooAngry;
    private int _booAngerStatus;
    private int _booCalmStatus;

    public delegate void ChangeAngerLevel();
    public event ChangeAngerLevel OnChangeAngerLevel;

    void Start()
    {
        
        if (_spriteList.Count <= 0)
        {
            return;
        }
        else if (_joystickImage == null)
        {
            return;
        }

        _booAngerStatus = AngrySystem.Instance.AngryLimits;
        _booCalmStatus = AngrySystem.Instance.CalmLimits;
        _isBooAngry = false;

        int index = _isBooAngry ? _booCalmStatus : _spriteList.Count - _booAngerStatus - 1;

        if (index >= 0 && index < _spriteList.Count && _spriteList[index] != null)
        {
            _joystickImage.sprite = _spriteList[index];
        }
        else
        {
        }

    }

    void Update()
    {
        bool newIsAngry = AngrySystem.Instance.IsAngry;
        if (newIsAngry != _isBooAngry)
        {
            _isBooAngry = newIsAngry;
            UpdateSprite();
        }

        if (_booAngerStatus != AngrySystem.Instance.AngryLimits || _booCalmStatus != AngrySystem.Instance.CalmLimits)
        {
            _booAngerStatus = AngrySystem.Instance.AngryLimits;
            _booCalmStatus = AngrySystem.Instance.CalmLimits;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        int index;
        if (!_isBooAngry)
        {
            index = _spriteList.Count - AngrySystem.Instance.AngryLimits - 1;
            Debug.Log($"[JoystickAnger] IsAngry: {_isBooAngry}, Angry Sprite Index: {index}");


        }
        else
        {
            index = AngrySystem.Instance.CalmLimits;
            Debug.Log($"[JoystickAnger] IsAngry: {_isBooAngry}, Calm Sprite Index: {index}");
        }

        if (index >= 0 && index < _spriteList.Count)
        {
            Debug.Log($"[JoystickAnger] Sprite applied: {_spriteList[index].name}");
            _joystickImage.sprite = _spriteList[index];
            OnChangeAngerLevel?.Invoke();
        }
        else
        {
            Debug.LogError($"[JoystickAnger] Index {index} invalide ou sprite manquant.");
        }


    }

}
