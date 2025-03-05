using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Unity.VisualScripting;

[DefaultExecutionOrder(-9)]
public class InputManager : Singleton<InputManager>
{
    private PlayerControls _controls;
    

    public bool IsController;

    private bool _characterEnabled;

    #region Events

    public delegate void ChangeDeviceEvent();
    public event ChangeDeviceEvent OnChangedDevice;

    public delegate void DashEvent();
    public event DashEvent OnDash;

    public delegate void InteractEvent();
    public event InteractEvent OnInteract;


    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();

        _characterEnabled = true;
       
    }

    private void OnEnable()
    {
        _controls.Enable();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        _controls.Disable();
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    #region Device Change

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Enabled:
                break;

            case InputDeviceChange.Removed:
            case InputDeviceChange.Disabled:
                break;
        }
    }

    private void UpdateControlMethod(InputControl control)
    {
        if (control == null) return;

        bool lastChange = IsController;

        IsController = control.device is Gamepad || control.device is Joystick;

        if (lastChange == IsController) return;

        OnChangedDevice?.Invoke();
    }


    #endregion


    #region Public Readers

    public Vector2 GetMoveDirection()
    {
        var moveDirection = _controls.Player.Move.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero)
        {
            var activeControl = GetActiveControl(_controls.Player.Move);
            UpdateControlMethod(activeControl);
        }
        return moveDirection;
    }

    private InputControl GetActiveControl(InputAction action)
    {
        InputControl activeControl = null;
        float maxValue = 0f;

        foreach (var control in action.controls)
        {
            if (control is AxisControl axisControl)
            {
                float controlValue = axisControl.ReadValue();
                if (Mathf.Abs(controlValue) > maxValue)
                {
                    maxValue = Mathf.Abs(controlValue);
                    activeControl = control;
                }
            }
        }

        return activeControl;
    }
    #endregion


}