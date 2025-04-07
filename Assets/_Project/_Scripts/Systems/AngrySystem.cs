using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngrySystem : Singleton<AngrySystem>
{
    private int _remainingLives;
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
        _remainingLives = 3;
    }
    [ContextMenu("Change Angry Limits")]
    public void ChangeAngryLimits()
    {
        if (_angryLimits > 0)
            _angryLimits--;

        if (_angryLimits == 0 && !IsAngry)
        {
            _angryLimits = _baseAngryLimits;

            StartCoroutine(WaitForChange());

            OnChangeElements?.Invoke();
            IsAngry = true;
            _angryLimits = _baseAngryLimits;

            VibrationSystem.Instance.VibratePhone(100.0f, 3.0f);
        }
    }

    private IEnumerator WaitForChange()
    {
        InputManager.Instance.Controls.Disable();

        yield return new WaitForSeconds(4);

        InputManager.Instance.Controls.Enable();
    }

    [ContextMenu("Reset Calm Limits")]
    public void ChangeCalmLimits()
    {
        if (_calmLimits > 0)
            _calmLimits--;

        if (_calmLimits == 0 && IsAngry)
        {
            _calmLimits = _baseCalmLimits;

            IsAngry = false;

            StartCoroutine(WaitForChange());

            OnResetElements?.Invoke();
            _calmLimits= _baseCalmLimits;

            _remainingLives--;
        }
    }

    private void Update()
    {
        CheckRemainingLives();
    }

    private void CheckRemainingLives()
    {
        if (_remainingLives <= 0)
        {
            // Faire charger l'écran de Lose ou reset la game directement mais on a pas encore ce qu'il faut donc petit debug log cadeau la famille

            Debug.Log("You got mad too many times... too bad");
        }
    }
}
