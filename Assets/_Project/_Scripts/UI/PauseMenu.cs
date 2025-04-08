using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    void Start()
    {
        _pauseButton.onClick.AddListener(Pause);
        _resumeButton.onClick.AddListener(Resume);
    }

    private void Pause()
    {
        Helpers.ShowCanva(GetComponent<CanvasGroup>());
        Time.timeScale = 0;
        InputManager.Instance.DisableSticksAndButtons();
    }

    private void Resume()
    {
        Helpers.HideCanva(GetComponent<CanvasGroup>());
        Time.timeScale = 1;
        InputManager.Instance.EnableSticksAndButtons();
    }
}
