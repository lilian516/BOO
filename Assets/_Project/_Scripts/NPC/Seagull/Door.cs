using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IClickable
{

    [SerializeField] Animator _animator;
    

    public bool IsKnocked = false;
    private bool _achivevementUnlock = false;
    public Vector3 PositionToGo { get; set; }


    private void Start()
    {
        PositionToGo = transform.GetChild(0).position;
    }
    public void OnClick()
    {
        _animator.SetTrigger("Vomit");
        IsKnocked = true;

        if (_achivevementUnlock)
            return;

        StartCoroutine(WaitForAchievement());
    }

    private IEnumerator WaitForAchievement()
    {
        yield return new WaitForSeconds(1.5f);

        AchievementSystem.Instance.SucceedAchievement(AchievementCondition.Knock_At_The_Door);
        _achivevementUnlock = true;
    }
}
