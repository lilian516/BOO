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
    public PlayerAutoMovingState AutoMovingState { get; set; }
    public PlayerAngryState AngryState { get; set; }
    public PlayerTakeSkillState TakeSkillState { get; set; }


    #endregion

    public InputManager Input {  get; set; }
    public Rigidbody RB { get; private set; }

    public Animator PlayerAnimator;
    public Animator PlayerFaceAnimator;

    [SerializeField] RuntimeAnimatorController _darkBoo;
    [SerializeField] RuntimeAnimatorController _boo;
    [SerializeField] AnimEventPlayer _eventPlayer;

    public float DetectorRadius;
    public GameObject DirectionalIndicator;

    private Inventory _inventory;
    [HideInInspector] public bool CanWalkForward;

    [Header("State Descriptors")]
    [Space(10f)]

    [SerializeField] PlayerIdleState.Descriptor _playerIdleStateDescriptor;
    [SerializeField] PlayerMovingState.Descriptor _playerMovingStateDescriptor;
    [SerializeField] PlayerAutoMovingState.Descriptor _playerAutoMovingStateDescriptor;
    [SerializeField] PlayerAngryState.Descriptor _playerAngryDescriptor;


    [Header("Skills Descriptors")]
    [Space(10f)]

    [SerializeField] SkillDescriptor _smashSkillDescriptor;

    private SmashSkill _smashSkill;

    private AnimatorOverrideController _overrideController;

    public Vector3 LookDir;
    public Vector3 SkillDir;

    public Vector3 PositionToGo { get; set; }

    public IClickable CurrentClickable { get; set; }
    public AnimEventPlayer EventPlayer { get => _eventPlayer; set => _eventPlayer = value; }

    [HideInInspector] public int CurrentTriggerLevel;

    #region Velocity

    [SerializeField] float _maxSpeed;

    private float _minSpeed;

    public float CurrentSpeed;

    #endregion


    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, _playerIdleStateDescriptor);
        MovingState = new PlayerMovingState(this, StateMachine, _playerMovingStateDescriptor);
        SkillState = new PlayerSkillState(this, StateMachine);
        SpeakingState = new PlayerSpeakingState(this, StateMachine);
        WaitingState = new PlayerWaitingState(this, StateMachine);
        AutoMovingState = new PlayerAutoMovingState(this, StateMachine, _playerAutoMovingStateDescriptor);
        AngryState = new PlayerAngryState(this, StateMachine, _playerAngryDescriptor);
        TakeSkillState = new PlayerTakeSkillState(this, StateMachine);
        _inventory = GetComponent<Inventory>();    

        _smashSkill = new SmashSkill(this, _smashSkillDescriptor);
        _inventory.AddSkill(_smashSkill, PlayerSkill.SmashSkill, true);
        RB = GetComponent<Rigidbody>();

        _minSpeed = _playerMovingStateDescriptor.Speed;
        CurrentSpeed = _minSpeed;

        CanWalkForward = true;
        CurrentTriggerLevel = 0;
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

        NPC_Detector detector = transform.GetComponentInChildren<NPC_Detector>();
        detector.SetDetectorRadius(DetectorRadius);
        detector.OnDetectNPC += ChangeAnimatorToCurious;
        detector.OnStopDetectNPC += ResetAnimatorLayer;

        if (!HasSkillSelected())
            DirectionalIndicator.SetActive(false);

        AngrySystem.Instance.OnFirstAngerOccurence += ChangeAnimatorToTriggerOne;
        AngrySystem.Instance.OnSecondAngerOccurence += ChangeAnimatorToTriggerTwo;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.ChangeStateChecks();
        StateMachine.CurrentState.FrameUpdate();

        RotateDirectionalIndicator();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
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

    public bool HasSkillSelected()
    {
        return !(_inventory.CurrentSkill == null);
    }

    public bool UseCurrentSkill()
    {
        if (_inventory.CurrentSkill != null)
        {
            _inventory.CurrentSkill.UseSkill();

           
            return true;
        }
        return false;
    }

    public bool StartUseCurrentSkill()
    {
        if (_inventory.CurrentSkill != null)
        {
            //_inventory.CurrentSkill.UseSkill();

            PlayerFaceAnimator.enabled = false;
            PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;

            _overrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);

            _overrideController["A_Boo_BubbleSkill"] = _inventory.CurrentSkill.AnimationSkill;
            PlayerAnimator.runtimeAnimatorController = _overrideController;
            //Debug.Log(_inventory.CurrentSkill);
            PlayerAnimator.SetTrigger("UseSkill");
            return true;
        }
        return false;
    }

    public void AddSkill(PlayerSkill playerSkill, SkillDescriptor descriptor)
    {
        if (!HasSkillSelected())
            DirectionalIndicator.SetActive(true);

        switch (playerSkill)
        {

            case PlayerSkill.Orb:
                break;
            case PlayerSkill.BubbleSkill:
                BubbleSkill bubbleSkill = new BubbleSkill(this, descriptor);
                _inventory.AddSkill(bubbleSkill, playerSkill);
                
                break;
            case PlayerSkill.PantsSkill:
                PantsSkill pantsSkill = new PantsSkill(this, descriptor);
                _inventory.AddSkill(pantsSkill, playerSkill);
                break;
            case PlayerSkill.StickSkill:
                StickSkill stickSkill = new StickSkill(this, descriptor);
                _inventory.AddSkill(stickSkill, playerSkill);
                break;
            case PlayerSkill.WindSkill:
                WindmillSkill windSkill = new WindmillSkill(this, descriptor);
                _inventory.AddSkill(windSkill, playerSkill);
                break;
        }
        ChangeAnimTakeSkill(descriptor.TakeAnimation);
        StateMachine.ChangeState(TakeSkillState);

        

    }

    private void ChangeAnimTakeSkill(AnimationClip animationClipTake)
    {
        _overrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);

        _overrideController["A_Boo_TakeStick"] = animationClipTake;
        PlayerAnimator.runtimeAnimatorController = _overrideController;
    }

    public void ChangeAnimAngry(AnimationClip animationClipAngry)
    {

        _overrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);

        _overrideController["A_Boo_Angry"] = animationClipAngry;
        PlayerAnimator.runtimeAnimatorController = _overrideController;
    }
    public void RemoveSkill(PlayerSkill playerSkill)
    {
        if (!_inventory.PlayerSkills.Contains(playerSkill))
            return;

        _inventory.RemoveSkill(playerSkill);

    }
    public void Change()
    {
        AddSkill(PlayerSkill.SmashSkill, _smashSkillDescriptor);

        
        //PlayerAnimator.runtimeAnimatorController = _darkBoo;
       
        

        CurrentSpeed = _maxSpeed;
        //StateMachine.ChangeState(WaitingState);
        
        PlayerAnimator.SetTrigger("IsAngry");
        PlayerFaceAnimator.enabled = false;
        PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(WaitBeforeAngry());
    }
    IEnumerator WaitBeforeAngry()
    {
        yield return new WaitForSeconds(2.3f);
        PlayerAnimator.runtimeAnimatorController = _darkBoo;
        //StateMachine.ChangeState(IdleState);
    }
    public void ResetChange()
    {
        EventPlayer.OnExitUseSkill += ChangeAnimatorToCalm;
        ChangeAnimatorToNormal();
    }
    private void ChangeAnimatorToCalm()
    {
  
        EventPlayer.OnExitUseSkill -= ChangeAnimatorToCalm;
        RemoveSkill(PlayerSkill.SmashSkill);
        PlayerAnimator.runtimeAnimatorController = _boo;
        PlayerFaceAnimator.enabled = true;

        CurrentSpeed = _minSpeed;
    }
    private void ChangeAnimatorToCurious()
    {
        PlayerFaceAnimator.SetLayerWeight(0,0);
        PlayerFaceAnimator.SetLayerWeight(1,1);
        PlayerFaceAnimator.SetLayerWeight(2, 0);
        PlayerFaceAnimator.SetLayerWeight(3, 0);
    }
    private void ChangeAnimatorToNormal()
    {
        PlayerFaceAnimator.SetLayerWeight(0, 1);
        PlayerFaceAnimator.SetLayerWeight(1, 0);
        PlayerFaceAnimator.SetLayerWeight(2, 0);
        PlayerFaceAnimator.SetLayerWeight(3, 0);

        CurrentTriggerLevel = 0;
    }

    public void ChangeAnimatorToTriggerOne()
    {
        PlayerFaceAnimator.SetLayerWeight(0, 0);
        PlayerFaceAnimator.SetLayerWeight(1, 0);
        PlayerFaceAnimator.SetLayerWeight(2, 1);
        PlayerFaceAnimator.SetLayerWeight(3, 0);

        CurrentTriggerLevel = 1;
    }

    public void ChangeAnimatorToTriggerTwo()
    {
        PlayerFaceAnimator.SetLayerWeight(0, 0);
        PlayerFaceAnimator.SetLayerWeight(1, 0);
        PlayerFaceAnimator.SetLayerWeight(2, 0);
        PlayerFaceAnimator.SetLayerWeight(3, 1);

        CurrentTriggerLevel = 2;
    }

    private void ResetAnimatorLayer()
    {
        switch (CurrentTriggerLevel)
        {
            case 0:
                ChangeAnimatorToNormal();
                break;
            case 1: 
                ChangeAnimatorToTriggerOne();
                break;
            case 2:
                ChangeAnimatorToTriggerTwo();
                break;
            default:
                break;
        }
    }

    private void RotateDirectionalIndicator()
    {
        if (InputManager.Instance.GetSelectDirection() != Vector2.zero)
        {
            SkillDir = new Vector3(InputManager.Instance.GetSelectDirection().x, 0, InputManager.Instance.GetSelectDirection().y);
            Quaternion targetRotation = Quaternion.LookRotation(SkillDir);
            DirectionalIndicator.transform.rotation = targetRotation;
        }
        
    }
}
