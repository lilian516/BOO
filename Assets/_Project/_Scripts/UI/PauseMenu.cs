using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] Animator _animatorTransition;
    void Start()
    {
        _pauseButton.onClick.AddListener(Pause);
    }

    private void Pause()
    {
        _animatorTransition.SetTrigger("Click");
        StartCoroutine(WaitOpenPause());
    }

    private IEnumerator WaitOpenPause()
    {
        yield return new WaitForSeconds(1.2f);

        InputManager.Instance.DisableControllerStick();
        InputManager.Instance.DisableSkillStick();
        StartCoroutine(GameManager.Instance.BackToMainMenu());
    }
}
