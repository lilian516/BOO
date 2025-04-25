using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventMauvis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundMauvisSad()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Mauvis Sad One", "Mauvis Sad Two", "Mauvis Sad Three", "Mauvis Fear One", "Mauvis Fear Two" },
            transform.position);
    }

    public void SoundMauvisFear()
    {
        
    }
}
