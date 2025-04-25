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

    public delegate void EndAnimation();
    public event EndAnimation OnEndAnimation;

    private float _animTime;

    public InputManager Input {  get; set; }
    public Rigidbody RB { get; private set; }

    public Animator PlayerAnimator;
    public Animator PlayerFaceAnimator;

    [SerializeField] RuntimeAnimatorController _darkBoo;
    [SerializeField] RuntimeAnimatorController _boo;
    [SerializeField] AnimEventPlayer _eventPlayer;

    public float DetectorRadius;
    public GameObject DirectionalIndicator;
    public GameObject DirectionalCapsule;
    [SerializeField] public Vector3 DirectionalCapsuleOffset;

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
    public bool FacingRight = true;


    public GameObject KillSheepVFX;

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
        _inventory.SetAngrySkill(_smashSkill);
        RB = GetComponent<Rigidbody>();

        _minSpeed = _playerMovingStateDescriptor.Speed;
        CurrentSpeed = _minSpeed;

        CanWalkForward = true;
        CurrentTriggerLevel = 0;
    

        DirectionalCapsuleOffset = DirectionalCapsule.transform.localPosition;
        DirectionalIndicator.SetActive(false);

        SkillDir = new Vector3(1,0,0);

    }

    void Start()
    {

        Input = InputManager.Instance;

        StateMachine.Initialize(WaitingState);

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        NPC_Detector detector = transform.GetComponentInChildren<NPC_Detector>();
        detector.SetDetectorRadius(DetectorRadius);
        detector.OnDetectNPC += ChangeAnimatorToCurious;
        detector.OnStopDetectNPC += ResetAnimatorLayer;

        AngrySystem.Instance.OnFirstAngerOccurence += ChangeAnimatorToTriggerOne;
        AngrySystem.Instance.OnSecondAngerOccurence += ChangeAnimatorToTriggerTwo;

        PlayerAnimator.enabled = false;
        PlayerAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        PlayerFaceAnimator.enabled = false;
        PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

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
        if (_inventory.CurrentSkill != null && SkillDir != Vector3.zero)
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
            _inventory.CurrentSkill.StartUseSkill();

            _overrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);

            
            
            if (!AngrySystem.Instance.IsAngry)
            {
                _overrideController["A_Boo_BubbleSkill"] = _inventory.CurrentSkill.AnimationSkill;
                PlayerAnimator.runtimeAnimatorController = _overrideController;

                PlayerAnimator.SetTrigger("UseSkill");

                StartCoroutine(WaitDisableFaceAnimator());
                _animTime = (_overrideController["A_Boo_BubbleSkill"].length - 0.01f) * 2;
            }


            else
            {
                _overrideController["A_Boo_KillSheep"] = _inventory.CurrentSkill.AnimationSkill;
                PlayerAnimator.runtimeAnimatorController = _overrideController;

                PlayerAnimator.SetTrigger("UseSkill");

                StartCoroutine(WaitDisableFaceAnimator());
                _animTime = (_overrideController["A_Boo_KillSheep"].length - 0.01f) * 2;
            }
                

            return true;
        }
        return false;
    }

    IEnumerator WaitDisableFaceAnimator()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerFaceAnimator.enabled = false;
        PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void AddSkill(PlayerSkill playerSkill, SkillDescriptor descriptor)
    {
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

        _animTime = (_overrideController["A_Boo_Angry"].length - 0.01f) * 2;
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

        CurrentSpeed = _maxSpeed;

        SoundSystem.Instance.ChangeMusicByKey("Dark Music");

        PlayerAnimator.SetTrigger("IsAngry");
        PlayerFaceAnimator.enabled = false;
        PlayerFaceAnimator.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(WaitBeforeAngry());
    }
    IEnumerator WaitBeforeAngry()
    {
        yield return new WaitForSeconds(2.3f);
        PlayerAnimator.runtimeAnimatorController = _darkBoo;
    }
    public void ResetChange()
    {
        OnEndAnimation += ChangeAnimatorToCalm;
        ChangeAnimatorToNormal();
        SoundSystem.Instance.ChangeMusicByKey("Chill Music");
    }
    private void ChangeAnimatorToCalm()
    {
        OnEndAnimation -= ChangeAnimatorToCalm;
        PlayerAnimator.SetTrigger("Transform");
        PlayerFaceAnimator.enabled = false;
        StartCoroutine(WaitBeforeHappy());
        
    }

    IEnumerator WaitBeforeHappy()
    {
        yield return new WaitForSeconds(1.23f);
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
        
        else if (InputManager.Instance.GetMoveDirection() != Vector2.zero)
        {
            SkillDir = LookDir;
            Quaternion targetRotation = Quaternion.LookRotation(LookDir);
            DirectionalIndicator.transform.rotation = targetRotation;
        }
    }

    public void WaitForSkillAnimation()
    {
        StartCoroutine(WaitEndAnim());
    }

    private IEnumerator WaitEndAnim()
    {
        yield return new WaitForSeconds(_animTime);

        OnEndAnimation?.Invoke();
    }

    private IEnumerator ChangeToBaseLayer()
    {
        yield return new WaitForSeconds(_animTime);

        PlayerAnimator.SetLayerWeight(0, 0);
        PlayerAnimator.SetLayerWeight(1, 1);
        PlayerFaceAnimator.enabled = true;

        StateMachine.ChangeState(IdleState);

        DirectionalIndicator.SetActive(true);
    }

    public void StartAnim()
    {
        PlayerAnimator.SetTrigger("Start");
        _overrideController = new AnimatorOverrideController(PlayerAnimator.runtimeAnimatorController);

        _animTime = _overrideController["A_Boo_GetUp"].length * 2;

        StartCoroutine(ChangeToBaseLayer());

    public bool HasSkill()
    {
        return _inventory.PlayerSkills.Count > 0;
    }
}
