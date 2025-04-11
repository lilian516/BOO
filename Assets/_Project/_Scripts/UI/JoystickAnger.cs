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
        // Si on est en colère, utiliser AngryLimits
        if (!_isBooAngry)
        {
            int index = _spriteList.Count - AngrySystem.Instance.AngryLimits  - 1;

            if (index >= 0 && index < _spriteList.Count)
            {
                _joystickImage.sprite = _spriteList[index];
            }
            else
            {
            }
        }
        else
        {
            int index = AngrySystem.Instance.CalmLimits;

            if (index >= 0 && index < _spriteList.Count)
            {
                _joystickImage.sprite = _spriteList[index];
            }
            else
            {
            }
        }

        
    }
}
