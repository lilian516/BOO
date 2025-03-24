using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    #region State Machine Variables

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMovingState MovingState { get; set; }
    public PlayerSkillState SkillState { get; set; }


    #endregion



    private Skill _currentSkill;
    public InputManager Input {  get; set; }
    public Rigidbody RB { get; private set; }
    public Skill CurrentSkill { get => _currentSkill; set => _currentSkill = value; }

    public float Speed;

    [Header("Skills Descriptors")]
    [Space(10f)]

    [SerializeField] BubbleSkill.Descriptor _bubbleSkillDescriptor;
    [SerializeField] StickSkill.Descriptor _stickSkillDescriptor;
    [SerializeField] WindmillSkill.Descriptor _windSkillDescriptor;



    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MovingState = new PlayerMovingState(this, StateMachine);
        SkillState = new PlayerSkillState(this, StateMachine);




        //BubbleSkill skill = new BubbleSkill(this, _bubbleSkillDescriptor);

        StickSkill stickSkill = new StickSkill(this, _stickSkillDescriptor);
        AddSkill(stickSkill);
        BubbleSkill bubbleSkill = new BubbleSkill(this, _bubbleSkillDescriptor);
        WindmillSkill windSkill = new WindmillSkill(this, _windSkillDescriptor);
        AddSkill(bubbleSkill);
        AddSkill(windSkill);

        
        RB = GetComponent<Rigidbody>();
        Speed = 5f;
    }
    // Start is called before the first frame update
    void Start()
    {

        Input = InputManager.Instance;
        // 9 minutes 37 video tuto

        StateMachine.Initialize(IdleState);
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
        if(_currentSkill != null)
        {
            _currentSkill.UseSkill();
        }
    }

    public void AddSkill(Skill skill)
    {
        //skill = new Skill(this);
        _currentSkill = skill;
    }
}
