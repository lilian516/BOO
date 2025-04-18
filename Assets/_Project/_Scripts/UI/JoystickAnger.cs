using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAnger : MonoBehaviour, IChangeable
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
        AngrySystem.Instance.OnFirstAngerOccurence += RefreshAngerUI;
        AngrySystem.Instance.OnSecondAngerOccurence += RefreshAngerUI;
        AngrySystem.Instance.OnChangeElements += Change;

        AngrySystem.Instance.OnFirstCalmOccurence += RefreshAngerUI;
        AngrySystem.Instance.OnSecondCalmOccurence += RefreshAngerUI;
        AngrySystem.Instance.OnResetElements += ResetChange;

    }
    public void RefreshAngerUI()
    {
        int index = 0;
        _isBooAngry = AngrySystem.Instance.IsAngry;
        if (_isBooAngry)
        {
            _isBooAngry = newIsAngry;
            SaveSystem.Instance.ResetAllData();
            UpdateSprite();
            return;
        }

        if (_booAngerStatus != AngrySystem.Instance.AngryLimits || _booCalmStatus != AngrySystem.Instance.CalmLimits)
        {
            _booAngerStatus = AngrySystem.Instance.AngryLimits;
            _booCalmStatus = AngrySystem.Instance.CalmLimits;
            if (!_isBooAngry)
            { 
                SaveSystem.Instance.SaveElement<bool>("isAngry", _isBooAngry);
                SaveSystem.Instance.SaveElement<int>("AngerStatus", _booAngerStatus);
                SaveSystem.Instance.SaveElement<int>("CalmStatus", _booCalmStatus);
            }
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        if (!_isBooAngry)
        {
            int index = _spriteList.Count - AngrySystem.Instance.AngryLimits  - 1;

            if (index >= 0 && index < _spriteList.Count)
            {
                _joystickImage.sprite = _spriteList[index];
            }
        }
        else
        {
            int index = AngrySystem.Instance.CalmLimits;

            if (index >= 0 && index < _spriteList.Count)
            {
                _joystickImage.sprite = _spriteList[index];
            }
        }
    }

    void UpdateSprite(int index)
    {
        if (index < 0 ||index >= _spriteList.Count)
        {
            Debug.LogError("Souci d'index, la fonction ne peut fonctionner.");
            return;
        }
        else
        {
            _joystickImage.sprite = _spriteList[index];
        }
    }

    public void Change()
    {
        UpdateSprite(3);
    }

    public void ResetChange()
    {
        UpdateSprite(0);
    }

}
