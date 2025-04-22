using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IClickable
{

    [SerializeField] Animator _animator;
    [SerializeField] Animator _animatorDoor;
    

    public bool IsKnocked = false;
    private bool _achivevementUnlock = false;
    public Vector3 PositionToGo { get; set; }
    public bool CanGoTo { get; set; }


    private void Start()
    {
        PositionToGo = transform.GetChild(0).position;
        CanGoTo = true;
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
        _animator.SetTrigger("Vomit");
        _animatorDoor.SetTrigger("Knock");
        IsKnocked = true;

        if (_achivevementUnlock)
            return;

        StartCoroutine(WaitForAchievement());
    }
}
