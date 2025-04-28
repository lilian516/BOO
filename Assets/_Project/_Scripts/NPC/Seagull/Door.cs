using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


public class Door : MonoBehaviour, IClickable, IDetectable
{

    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorDoor;
    [SerializeField] Animator _animatorDoorModel;
    [SerializeField] Material _doorOutlineMaterial;

    public bool IsKnocked = false;
    private bool _achivevementUnlock = false;
    public Vector3 PositionToGo { get; set; }
    public bool CanGoTo { get; set; }
    public bool NeedToFaceRight { get; set; }

    private void Start()
    {
        PositionToGo = transform.GetChild(0).position;
        CanGoTo = true;
        NeedToFaceRight = false;
    }

    public void OnClick()
    {
       
    }

    private IEnumerator WaitForAchievement()
    {
        yield return new WaitForSeconds(1.5f);

        AchievementSystem.Instance.SucceedAchievement(AchievementCondition.Knock_At_The_Door);
        _achivevementUnlock = true;
    }

    public void OnDestinationReached()
    {
        Player player = GameManager.Instance.Player.GetComponent<Player>();
        if (player.FacingRight)
        {
            SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();
            sprites[0].flipX = true;
            sprites[1].flipX = true;
        }

        _animator.SetTrigger("Vomit");
        _animatorDoor.SetTrigger("Knock");
        //_animatorDoorModel.SetTrigger("Knock");
        IsKnocked = true;

        if (_achivevementUnlock)
            return;

        StartCoroutine(WaitForAchievement());
    }

    public void Detected()
    {
        if (!AngrySystem.Instance.IsAngry)
        {
            _doorOutlineMaterial.SetFloat("_Outline_Thickness", 0.01f);
            return;
        }
        NoDetected();
    }

    public void NoDetected()
    {
        _doorOutlineMaterial.SetFloat("_Outline_Thickness", 0.0f);
    }
}
