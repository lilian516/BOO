using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    [SerializeField] GameObject _resetButton;
    [SerializeField] GameObject _confirmBox;
    [SerializeField] GameObject _animatorObject;
    private Animator _animator;

    private delegate void EndAnim();
    private event EndAnim OnEndAnim;

    private bool _confirmOpen;

    private void Start()
    {
        _animator = _animatorObject.GetComponent<Animator>();
    }

    public void ClickButton()
    {
        if (_confirmOpen)
            return;

        _confirmOpen = true;

        _animator.SetTrigger("Click");
        Helpers.HideCanva(_resetButton.GetComponent<CanvasGroup>());
        Helpers.ShowCanva(_animatorObject.GetComponent<CanvasGroup>());

        StartCoroutine(WaitForEndAnim(0.5f,true));
    }

    public void ConfirmReset()
    {
        _animator.SetTrigger("Confirm");
        Helpers.HideCanva(_confirmBox.GetComponent<CanvasGroup>());
        Helpers.HideCanva(_resetButton.GetComponent<CanvasGroup>());
        Helpers.ShowCanva(_animatorObject.GetComponent<CanvasGroup>());

        OnEndAnim += GameManager.Instance.ResetGame;

        StartCoroutine(WaitForEndAnim(0.9f, false));
    }

    public void CancelReset()
    {
        _animator.SetTrigger("Cancel"); 
        Helpers.HideCanva(_confirmBox.GetComponent<CanvasGroup>());
        Helpers.HideCanva(_resetButton.GetComponent<CanvasGroup>());
        Helpers.ShowCanva(_animatorObject.GetComponent<CanvasGroup>());

        _confirmOpen = false;

        StartCoroutine(WaitForEndAnim(0.5f, false));
    }

    private IEnumerator WaitForEndAnim(float animTime, bool showBox)
    {
        yield return new WaitForSeconds(animTime);

        Helpers.ShowCanva(_resetButton.GetComponent<CanvasGroup>());
        Helpers.HideCanva(_animatorObject.GetComponent<CanvasGroup>());

        if (showBox)
            Helpers.ShowCanva(_confirmBox.GetComponent<CanvasGroup>());
        else
            Helpers.HideCanva(_confirmBox.GetComponent<CanvasGroup>());

        OnEndAnim?.Invoke();
    }
}
