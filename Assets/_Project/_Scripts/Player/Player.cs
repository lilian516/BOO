using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour, IChangeable
{


    #region State Machine Variables

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMovingState MovingState { get; set; }
    public PlayerSkillState SkillState { get; set; }
    public PlayerSpeakingState SpeakingState { get; set; }
    public PlayerWaitingState WaitingState { get; set; }


    #endregion

    public InputManager Input {  get; set; }
    public Rigidbody RB { get; private set; }

    public Animator PlayerAnimator;
    public Animator PlayerFaceAnimator;

    [SerializeField] RuntimeAnimatorController _darkBoo;
    [SerializeField] RuntimeAnimatorController _boo;
    

    private Inventory _inventory;

    [Header("State Descriptors")]
    [Space(10f)]

    [SerializeField] PlayerIdleState.Descriptor _playerIdleStateDescriptor;
    [SerializeField] PlayerMovingState.Descriptor _playerMovingStateDescriptor;
    

    [Header("Skills Descriptors")]
    [Space(10f)]

    [SerializeField] BubbleSkill.Descriptor _bubbleSkillDescriptor;
    [SerializeField] StickSkill.Descriptor _stickSkillDescriptor;
    [SerializeField] WindmillSkill.Descriptor _windSkillDescriptor;
    [SerializeField] PantsSkill.Descriptor _pantsSkillDescriptor;
    [SerializeField] SmashSkill.Descriptor _smashSkillDescriptor;

    private SmashSkill _smashSkill;

    public Vector3 LookDir;
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, _playerIdleStateDescriptor);
        MovingState = new PlayerMovingState(this, StateMachine, _playerMovingStateDescriptor);
        SkillState = new PlayerSkillState(this, StateMachine);
        SpeakingState = new PlayerSpeakingState(this, StateMachine);
        WaitingState = new PlayerWaitingState(this, StateMachine);
        _inventory = GetComponent<Inventory>();    

        _smashSkill = new SmashSkill(this, _smashSkillDescriptor);
        
        RB = GetComponent<Rigidbody>();
        
       
    }
    // Start is called before the first frame update
    void Start()
    {

        Input = InputManager.Instance;
        // 9 minutes 37 video tuto

        StateMachine.Initialize(IdleState);

        
        //Input.OnSkillMenu += SelectSkill;
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentPlayerState.ChangeStateChecks();
        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    public bool IsMoving()
    {
        return (Input.GetMoveDirection() != Vector2.zero);
    }


    public Vector3 GetMoveDirection()
    {
        Camera camera = Camera.main;
        Vector3 cameraForward = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;

        Vector2 inputDirection = Input.GetMoveDirection().normalized;

        Vector3 movedir = (inputDirection.y * cameraForward) + (inputDirection.x * cameraRight);

        return movedir;
    }


    public void UseCurrentSkill()
    {
        if (!_inventory.SelectSkill())
            return;

        if (_inventory.CurrentSkill != null)
        {
            _inventory.CurrentSkill.UseSkill();
            PlayerAnimator.SetTrigger("UseSkill");
        }
    }

    public void AddSkill(PlayerSkill playerSkill)
    {

        switch(playerSkill)
        {
            case PlayerSkill.BubbleSkill:
                BubbleSkill bubbleSkill = new BubbleSkill(this, _bubbleSkillDescriptor);
                _inventory.AddSkill(bubbleSkill, playerSkill);
                break;
            case PlayerSkill.PantsSkill:
                PantsSkill pantsSkill = new PantsSkill(this, _pantsSkillDescriptor);
                _inventory.AddSkill(pantsSkill, playerSkill);
                break;
            case PlayerSkill.StickSkill:
                StickSkill stickSkill = new StickSkill(this, _stickSkillDescriptor);
                _inventory.AddSkill(stickSkill, playerSkill);
                break;
            case PlayerSkill.WindSkill:
                WindmillSkill windSkill = new WindmillSkill(this, _windSkillDescriptor);
                _inventory.AddSkill(windSkill, playerSkill);
                break;
            case PlayerSkill.SmashSkill:
                _inventory.AddSkill(_smashSkill, playerSkill);
                break;
        }

    }
    public void RemoveSkill(PlayerSkill playerSkill)
    {
        if (!_inventory.PlayerSkills.Contains(playerSkill))
            return;

        _inventory.RemoveSkill(playerSkill);

    }

    public void Change()
    {
        AddSkill(PlayerSkill.SmashSkill);
        PlayerAnimator.runtimeAnimatorController = _darkBoo;

    }
    public void ResetChange()
    {
        RemoveSkill(PlayerSkill.SmashSkill);
        PlayerAnimator.runtimeAnimatorController = _boo;
    }
}
