using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    private CanvasGroup _canvasMainMenuGroup;

    [SerializeField] private Toggle _french;
    [SerializeField] private Toggle _english;
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
        animator.SetTrigger("ClickParam");

        StartCoroutine(WaitOpenSettings());
    }

    private IEnumerator WaitOpenSettings()
    {
        SoundSystem.Instance.PlaySoundFXClipByKey("Global Book", 1f);
        yield return new WaitForSeconds(0.55f);

        Helpers.ShowCanva(_canvasGroup);
        Helpers.HideCanva(_canvasMainMenuGroup);
    }
    public void CloseSettings(Animator animator)
    {
        animator.SetTrigger("Click");

        StartCoroutine(WaitCloseSettings());
        //Helpers.HideCanva(_canvasGroup);
        
    }

    private IEnumerator WaitCloseSettings()
    {
        yield return new WaitForSeconds(0.40f);

        Helpers.HideCanva(_canvasGroup);
        Helpers.ShowCanva(_canvasMainMenuGroup);
    }

    public void SetMusicVolume(float level)
    {
        SoundMixerManager.Instance.SetMusicVolume(level);
    }

    public void SetSoundFXVolume(float level)
    {
        SoundMixerManager.Instance.SetSoundFXVolume(level);
    }


    public void ClickVibration(Animator animator)
    {
        animator.SetTrigger("Click");

        VibrationSystem.Instance.IsToggled = !VibrationSystem.Instance.IsToggled;
    }

    public void ChangeLanguage(string name)
    {
        if (name == "English")
        {
            _french.interactable = true;
            _english.interactable = false;
        }
        else if (name == "French")
        {
            _english.interactable = true;
            _french.interactable = false;
        }
    }
}
