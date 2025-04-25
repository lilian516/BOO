using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventRemumus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SoundRemumusBlow()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Remumus Soufle One", "Remumus Soufle Two", "Remumus Soufle Three" }, 
            transform.position,0.7f);
    }
    public void SoundRemumusFlies()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Remumus Mouche One", "Remumus Mouche Two", "Remumus Mouche Three", "Remumus Mouche Four", "Remumus Mouche Five" }, 
            transform.position, 0.7f);
    }
    public void SoundRemumusDent()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Remumus Dent One"}, transform.position,0.7f);
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Remumus Fear One", "Remumus Fear Two" }, transform.position, 0.7f);
    }

    public void SoundRemumusTakeSlip()
    {
        SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Remumus Takeslip One", "Remumus Takeslip Two", "Remumus Takeslip Three" }, 
            transform.position, 0.7f);
    }
}
