using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

public class UIUtility : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<string> audioParameters; 
    [SerializeField] private List<Slider> sliders;

    private bool CanManageAudio;

    public void Start()
    {
        CanManageAudio = true;
    }

    public void HideUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }

    public void ShoweUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
    }

    #region Audio
    public void ManageVolume()
    {
        if (CanManageAudio)
        {
            for (int index = 0; index < sliders.Count; index++)
            {
                audioMixer.SetFloat(audioParameters[index], Mathf.Log10(sliders[index].value) * 20);
            }
        }
    }

    public void SliderValueCorrespondToVolume()
    {
        CanManageAudio = false;
        for (int index = 0; index < sliders.Count; index++)
        {
            if (audioMixer.GetFloat(audioParameters[index], out float volume))
            {
                Debug.Log($"Volume for {audioParameters[index]}: {volume} dB");
                sliders[index].value = Mathf.Pow(10, volume / 20);
                Debug.Log($"Slider value set to: {sliders[index].value}");
            }
        }
        CanManageAudio = true;
    }
    #endregion

    #region Canvas

    public void DesactiveCanvas(Canvas ThisCanva)
    {
        CanvasGroup groupCanva = ThisCanva.GetComponent<CanvasGroup>();
        if (groupCanva != null)
        {
            groupCanva.alpha = 0;
            groupCanva.interactable = false;
            groupCanva.blocksRaycasts = false;
        }
    }

    public void ActivateCanvas(Canvas ThisCanva)
    {
        CanvasGroup groupCanva = ThisCanva.GetComponent<CanvasGroup>();
        if (groupCanva != null)
        {
            groupCanva.alpha = 1;
            groupCanva.interactable = true;
            groupCanva.blocksRaycasts = true;
        }
    }
    #endregion
}


