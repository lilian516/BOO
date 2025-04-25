using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimEventPlayer;

public class ChildEventPlayer : MonoBehaviour
{
    public delegate void EndWindAnim();
    public event EndWindAnim OnEndWindAnim;

    public void WindAnimInvoker()
    {
        OnEndWindAnim?.Invoke();
    }

    public void SoundChildSurprise()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Child Suprise One", "Child Suprise Two", "Child Suprise Three"},
            transform.position);
    }
    public void SoundChildFear()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Child Fear One", "Child Fear Two"},
            transform.position);
    }
    public void SoundChildBubble()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Bubble Child One", "Bubble Child Two" },
            transform.position);
    }

    public void SoundChildBlow()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Child Blow One", "Child Blow Two", "Child Blow Three", "Child Blow Four", "Child Blow Five", "Child Blow Six" },
            transform.position);
    }
}
