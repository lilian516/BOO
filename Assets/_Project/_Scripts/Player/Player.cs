using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{


    #region State Machine Variables

    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMovingState MovingState { get; set; }
    public PlayerSkillState SkillState { get; set; }
    public PlayerSpeakingState SpeakingState { get; set; }


    #endregion

    public InputManager Input {  get; set; }
    public Rigidbody RB { get; private set; }

    public Animator PlayerAnimator;

    [SerializeField] AnimatorController _darkBoo;
    

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

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, _playerIdleStateDescriptor);
        MovingState = new PlayerMovingState(this, StateMachine, _playerMovingStateDescriptor);
        SkillState = new PlayerSkillState(this, StateMachine);
        SpeakingState = new PlayerSpeakingState(this, StateMachine);

        _inventory = GetComponent<Inventory>();    

        StickSkill stickSkill = new StickSkill(this, _stickSkillDescriptor);  
        BubbleSkill bubbleSkill = new BubbleSkill(this, _bubbleSkillDescriptor);
        WindmillSkill windSkill = new WindmillSkill(this, _windSkillDescriptor);
        PantsSkill pantsSkill = new PantsSkill(this, _pantsSkillDescriptor);

        AddSkill(pantsSkill);
        AddSkill(stickSkill);
        AddSkill(bubbleSkill);
        AddSkill(windSkill);
        
        
        RB = GetComponent<Rigidbody>();
        
       
    }
    // Start is called before the first frame update
    void Start()
    {

        Input = InputManager.Instance;
        // 9 minutes 37 video tuto

        StateMachine.Initialize(IdleState);

        Input.OnOpenSkillMenu += CheckMenuIsOpen;
        Input.OnCloseSkillMenu += CloseSkillMenu;


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
        if(_inventory.CurrentSkill != null)
        {
            _inventory.CurrentSkill.UseSkill();
        }
    }

    public void AddSkill(Skill skill)
    {
        _inventory.AddSkill(skill);
        
    }


    private void CheckMenuIsOpen()
    {
        StartCoroutine(GetTimePerformedButton());
    }


    private IEnumerator GetTimePerformedButton()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
            if (!Input.GetPerformedButton())
            {
                yield break;
            }
        }
        Debug.Log("Le menu est ouvert !!");
        _inventory.OpenInventory();
    }

    private void CloseSkillMenu()
    {
        //_inventory.CloseInventory();
    }
}
