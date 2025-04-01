using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


[DefaultExecutionOrder(-9)]
public class InputManager : Singleton<InputManager>
{
    private PlayerControls _controls;
    

    public bool IsController;

    private bool _characterEnabled;

    public PlayerControls Controls { get => _controls; set => _controls = value; }

    #region Events
    public delegate void ChangeDeviceEvent();
    public event ChangeDeviceEvent OnChangedDevice;

    public delegate void DashEvent();
    public event DashEvent OnDash;

    public delegate void InteractEvent();
    public event InteractEvent OnInteract;


    public delegate void UseSkillEvent();
    public event UseSkillEvent OnUseSkill;

    public delegate void CheckSpeakEvent();
    public event CheckSpeakEvent OnSpeak;

    public delegate void OpenSkillMenuEvent();
    public event OpenSkillMenuEvent OnOpenSkillMenu;

    public delegate void CloseSkillMenuEvent();
    public event CloseSkillMenuEvent OnCloseSkillMenu;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();

        
        _characterEnabled = true;
        OnEnable();

    }

    private void OnEnable()
    {
        _controls.Enable();
        InputSystem.onDeviceChange += OnDeviceChange;
        BindCharacterEvents();
    }

    private void OnDisable()
    {
        _controls.Disable();
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void Update()
    {
    }


    private void BindCharacterEvents()
    {
        _controls.Player.UseSkill.started += ctx => { UpdateControlMethod(ctx.control); OpenSkillPerformed(); };
        _controls.Player.UseSkill.canceled += ctx => { UpdateControlMethod(ctx.control); UseSkillPerformed(); };
        _controls.Player.Speak.performed += ctx => { UpdateControlMethod(ctx.control); CheckSpeakingPerformed(); };
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
        Vector2 moveDirection = _controls.Player.Move.ReadValue<Vector2>();
        if (moveDirection != Vector2.zero)
        {
            var activeControl = GetActiveControl(_controls.Player.Move);
            UpdateControlMethod(activeControl);
        }
        return moveDirection;
    }

    public Vector2 GetSelectDirection()
    {
        Vector2 selectDirection = _controls.Player.SelectSkill.ReadValue<Vector2>();
        return selectDirection;
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


    public bool GetPerformedButton()
    {
        if (_controls.Player.UseSkill.ReadValue<float>() > 0) 
        {
            return true;
        }
        return false;
    }

    public Vector2 GetTouchPosition()
    {
        return _controls.Player.Touch.ReadValue<Vector2>();
    }


    
    #endregion


    #region Event Callers

    private void UseSkillPerformed()
    {

        OnUseSkill?.Invoke();
        OnCloseSkillMenu?.Invoke();
    }
    
    private void OpenSkillPerformed()
    {
        Debug.Log("started");
        OnOpenSkillMenu?.Invoke();
    }

    private void CheckSpeakingPerformed()
    {
        OnSpeak?.Invoke();
        //Vector2 test = _controls.Player.Speak.ReadValue<Vector2>();
        //Debug.Log(test);
    }

    #endregion

}