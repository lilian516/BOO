using UnityEngine;
using UnityEngine.Audio;
using static Cinemachine.DocumentationSortingAttribute;

public class SoundMixerManager : Singleton<SoundMixerManager>
{
    [SerializeField] private AudioMixer audioMixer;
    

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

        //audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

        //SaveSystem.Instance.SaveElement<float>("MusicVolume", level);
        //audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
        
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SoundFXVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

        //audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetAmbianceVolume(float level)
    {
        audioMixer.SetFloat("AmbianceVolume", level == 0f ? -80f : Mathf.Log10(level) * 20f);

        //audioMixer.SetFloat("AmbianceVolume", Mathf.Log10(level) * 20f);
    }
}
