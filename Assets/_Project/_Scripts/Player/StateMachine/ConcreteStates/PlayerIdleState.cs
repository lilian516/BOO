using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class PlayerIdleState : PlayerState
{
    private bool _isSpeakingToSomeone;
    
    [System.Serializable]
    public class Descriptor
    {
        public AnimationCurve SpeedCurve;
        public float DurationStop;
    }

    Descriptor _desc;
    
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, Descriptor desc) : base(player, playerStateMachine)
    {
        _desc = desc;
    }

    public override void EnterState()
    {
        _isSpeakingToSomeone = false;
        base.EnterState();

        _player.Input.OnSkillMenu += OnSkill;
        _player.Input.OnSpeak += OnCheckSpeak;
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.Input.OnSkillMenu -= OnSkill;
        _player.Input.OnSpeak -= OnCheckSpeak;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    public override void ChangeStateChecks()
    {
        base.ChangeStateChecks();

        if (_player.IsMoving())
        {
            _playerStateMachine.ChangeState(_player.MovingState);
        }
    }

    

    private void OnSkill()
    {
        _playerStateMachine.ChangeState(_player.SkillState);
    }

    private bool IsTouchOverUI(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };  

        List<RaycastResult> RaycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, RaycastResults);

        return RaycastResults.Count > 0;
    }

    private void OnCheckSpeak()
    {
        if (AngrySystem.Instance.IsAngry)
            return;

        Vector2 position = _player.Input.GetTouchPosition();

        if (IsTouchOverUI(position))
        {
            return;
        }



        if (!_isSpeakingToSomeone && Physics.Raycast(Camera.main.ScreenPointToRay(position), out RaycastHit hit))
        {
            ISpeakable speakable = hit.collider.gameObject.GetComponent<ISpeakable>();
            if (speakable != null)
            {
                if (Vector3.Distance(_player.transform.position, hit.transform.position) <= _player.DetectorRadius)
                {
                    _isSpeakingToSomeone = true;
                    speakable.Speak();
                    _playerStateMachine.ChangeState(_player.SpeakingState);
                }
                
            }
            IClickable clickable = hit.collider.gameObject.GetComponent<IClickable>();

            if(clickable != null)
            {
                if (Vector3.Distance(_player.transform.position, hit.transform.position) <= _player.DetectorRadius)
                {
                    
                    _player.CurrentClickable = clickable;
                    _player.PositionToGo = hit.transform.position;
                   _playerStateMachine.ChangeState(_player.AutoMovingState);
                   
                }
            }


            Orbe orbe = hit.collider.gameObject.GetComponent<Orbe>();
            if (orbe != null)
            {
                orbe.UseOrbe();
                _playerStateMachine.ChangeState(_player.WaitingState);

            }
        }
    }

}
