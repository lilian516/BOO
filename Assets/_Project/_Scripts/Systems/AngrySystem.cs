using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngrySystem : Singleton<AngrySystem>
{
    private int _angryLimits;
    private int _baseAngryLimits;
    private int _calmLimits;
    private int _baseCalmLimits;
    public bool IsAngry;

    public delegate void ChangeElements();
    public event ChangeElements OnChangeElements;

    public delegate void ResetChangedElements();
    public event ResetChangedElements OnResetElements;

    void Start()
    {
        IsAngry = false;
        _baseAngryLimits = 3;
        _baseCalmLimits = 3;
        _angryLimits = 3;
        _calmLimits = 3;
    }
    [ContextMenu("Change Angry Limits")]
    public void ChangeAngryLimits()
    {
        if (_angryLimits > 0)
            _angryLimits--;

        if (_angryLimits == 0)
        {
            OnChangeElements?.Invoke();
            IsAngry = true;
            _angryLimits = _baseAngryLimits;

            VibrationSystem.Instance.TriggerVibration(0.7f, 3.0f);
        }
    }

    [ContextMenu("Reset Calm Limits")]
    public void ChangeCalmLimits()
    {
        if (_calmLimits > 0)
            _calmLimits--;

        if (_calmLimits == 0)
        {
            IsAngry = false;
            OnResetElements?.Invoke();
            _calmLimits= _baseCalmLimits;
        }
    }


}
