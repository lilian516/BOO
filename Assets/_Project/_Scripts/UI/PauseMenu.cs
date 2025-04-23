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
        _animatorTransition.SetTrigger("Click");
        StartCoroutine(WaitOpenPause());
    }

    private IEnumerator WaitOpenPause()
    {
        
        yield return new WaitForSeconds(1.2f);

        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainMenu" });

        SaveSystem.Instance.SaveAllData();

        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "UIInGame" });
    }

    private void Resume()
    {
        Helpers.HideCanva(GetComponent<CanvasGroup>());
        Time.timeScale = 1;
        InputManager.Instance.EnableControllerSticks();
        _pauseButton.onClick.AddListener(Pause);
    }

    private void Quit()
    {
        Helpers.HideCanva(GetComponent<CanvasGroup>());
        StartCoroutine(GameManager.Instance.BackToMainMenu());
    }
}
