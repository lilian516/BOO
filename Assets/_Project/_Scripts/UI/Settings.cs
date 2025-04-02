using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettings()
    {

        Helpers.ShowCanva(_canvasGroup);
        
    }
    public void CloseSettings()
    {

        Helpers.HideCanva(_canvasGroup);
        
    }

    public void SetMusicVolume(float level)
    {
        SoundMixerManager.Instance.SetMusicVolume(level);
    }

    public void SetSoundFXVolume(float level)
    {
        SoundMixerManager.Instance.SetSoundFXVolume(level);
    }

}
