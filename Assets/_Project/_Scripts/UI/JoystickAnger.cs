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
        if (_joystickImage == null)
            return;

        int index = 0;
        _isBooAngry = AngrySystem.Instance.IsAngry;
        if (_isBooAngry)
        {
            index = AngrySystem.Instance.CalmLimits;
        }
        else
        {
            int angerLevel = AngrySystem.Instance.AngryLimits;
            switch(angerLevel)
            {
                case 3: index = 0;break;
                case 2: index = 1;  break;
                case 1: index = 2; break;
                case 0: index = 3;break;
            }
        }
        
        UpdateSprite(index);
    }

    void UpdateSprite(int index)
    {
        if (_joystickImage == null)
        {
            Debug.LogWarning("JoystickAnger: Image component manquant ou détruit.");
            return;
        }

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

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnFirstAngerOccurence -= RefreshAngerUI;
            AngrySystem.Instance.OnSecondAngerOccurence -= RefreshAngerUI;
            AngrySystem.Instance.OnChangeElements -= Change;

            AngrySystem.Instance.OnFirstCalmOccurence -= RefreshAngerUI;
            AngrySystem.Instance.OnSecondCalmOccurence -= RefreshAngerUI;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }
}
