using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAnimEventPlayer : MonoBehaviour
{
    public delegate void EndAttackAnim();
    public event EndAttackAnim OnEndAttackAnim;

    public void ExitAttackAnim()
    {
        OnEndAttackAnim?.Invoke();
    }
}
