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

    
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        MovingState = new PlayerMovingState(this, StateMachine);
        SkillState = new PlayerSkillState(this, StateMachine);


        _currentSkill = new Skill();
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
        _currentSkill.UseSkill();
    }
}
