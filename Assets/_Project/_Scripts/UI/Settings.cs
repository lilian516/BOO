using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    private CanvasGroup _canvasMainMenuGroup;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMainMenuCanvaGroup(CanvasGroup canvasGroup)
    {
        _canvasMainMenuGroup = canvasGroup;
    }
    public void OpenSettings(Animator animator)
    {
        animator.SetTrigger("Click");

        StartCoroutine(WaitOpenSettings());
    }

    private IEnumerator WaitOpenSettings()
    {
        yield return new WaitForSeconds(0.45f);

        Helpers.ShowCanva(_canvasGroup);
        Helpers.HideCanva(_canvasMainMenuGroup);
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
