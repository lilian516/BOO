using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] Animator _animatorTransition;
    void Start()
    {
        _pauseButton.onClick.AddListener(Pause);
        _resumeButton.onClick.AddListener(Resume);
        _quitButton.onClick.AddListener(Quit);
    }

    private void Pause()
    {
        //Helpers.ShowCanva(GetComponent<CanvasGroup>());
        _animatorTransition.SetTrigger("Click");
        _pauseButton.interactable = false;
        //Time.timeScale = 0;
        StartCoroutine(WaitOpenPause());
        InputManager.Instance.DisableControllerStick();
        InputManager.Instance.DisableSkillStick();
    }

    private IEnumerator WaitOpenPause()
    {
        yield return new WaitForSeconds(1.2f);

        Helpers.ShowCanva(GetComponent<CanvasGroup>());
        _pauseButton.interactable = true;
    }

    private void Resume()
    {
        Helpers.HideCanva(GetComponent<CanvasGroup>());
        Time.timeScale = 1;
        InputManager.Instance.EnableControllerSticks();
    }

    private void Quit()
    {
        Helpers.HideCanva(GetComponent<CanvasGroup>());
        StartCoroutine(GameManager.Instance.BackToMainMenu());
    }
}
