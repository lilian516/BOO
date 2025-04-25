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

    public void SoundBeeBite()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Bee Bite One", "Bee Bite Two", "Bee Bite Three"},
            transform.position);
    }
}
