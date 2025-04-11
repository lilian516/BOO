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


    #endregion

    public InputManager Input {  get; set; }
    public Rigidbody RB { get; private set; }

    public Animator PlayerAnimator;
    public Animator PlayerFaceAnimator;

    [SerializeField] RuntimeAnimatorController _darkBoo;
    [SerializeField] RuntimeAnimatorController _boo;

    public float DetectorRadius;
    public GameObject DirectionalIndicator;

    private Inventory _inventory;
    [HideInInspector] public bool CanWalkForward;

    [Header("State Descriptors")]
    [Space(10f)]

    [SerializeField] PlayerIdleState.Descriptor _playerIdleStateDescriptor;
    [SerializeField] PlayerMovingState.Descriptor _playerMovingStateDescriptor;
    [SerializeField] PlayerAutoMovingState.Descriptor _playerAutoMovingStateDescriptor;


    [Header("Skills Descriptors")]
    [Space(10f)]

    [SerializeField] BubbleSkill.Descriptor _bubbleSkillDescriptor;
    [SerializeField] StickSkill.Descriptor _stickSkillDescriptor;
    [SerializeField] WindmillSkill.Descriptor _windSkillDescriptor;
    [SerializeField] PantsSkill.Descriptor _pantsSkillDescriptor;
    [SerializeField] SmashSkill.Descriptor _smashSkillDescriptor;

    private SmashSkill _smashSkill;

    public Vector3 LookDir;

    public Vector3 PositionToGo { get; set; }

    public IClickable CurrentClickable { get; set; }

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

        _inventory = GetComponent<Inventory>();    

        _smashSkill = new SmashSkill(this, _smashSkillDescriptor);
        _inventory.AddSkill(_smashSkill, PlayerSkill.SmashSkill, true);
        RB = GetComponent<Rigidbody>();

        _minSpeed = _playerMovingStateDescriptor.Speed;
        CurrentSpeed = _minSpeed;

        CanWalkForward = true;
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
        detector.OnStopDetectNPC += ChangeAnimatorToNormal;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentPlayerState.ChangeStateChecks();
        StateMachine.CurrentPlayerState.FrameUpdate();

        RotateDirectionalIndicator();
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
        yield return new WaitForSeconds(1.1f);
        PlayerAnimator.runtimeAnimatorController = _darkBoo;
        //StateMachine.ChangeState(IdleState);
    }
    public void ResetChange()
    {
        RemoveSkill(PlayerSkill.SmashSkill);
        PlayerAnimator.runtimeAnimatorController = _boo;
        PlayerFaceAnimator.enabled = true;

        CurrentSpeed = _minSpeed;

    }

    private void ChangeAnimatorToCurious()
    {
        PlayerFaceAnimator.SetLayerWeight(0,0);
        PlayerFaceAnimator.SetLayerWeight(1,1);
    }
    private void ChangeAnimatorToNormal()
    {
        PlayerFaceAnimator.SetLayerWeight(0, 1);
        PlayerFaceAnimator.SetLayerWeight(1, 0);
    }

    private void RotateDirectionalIndicator()
    {
        Quaternion targetRotation = Quaternion.LookRotation(LookDir);

        DirectionalIndicator.transform.rotation = targetRotation;
    }
}
