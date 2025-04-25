using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventSheep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundSheepBele()
    {
        //SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Mouton Bele One", "Mouton Bele Two", "Mouton Bele Three", "Mouton Bele Four", "Mouton Bele Five" },
            //transform.position);
    }

    

    public void SoundSheepCurious()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Mouton Curious One", "Mouton Curious Two", "Mouton Curious Three" },
            transform.position);
    }

    public void SoundSheepBubble()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Mouton Buble One", "Mouton Buble Two", "Mouton Buble Three" },
            transform.position);
    }

}
