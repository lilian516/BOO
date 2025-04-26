using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    
    [SerializeField] Animator _animatorTransition;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton(Animator animator)
    {
        animator.SetTrigger("ClickPlay");

        Helpers.HideCanva(_canvasGroup);
        StartCoroutine(WaitEndAnim());
        
    }

    private IEnumerator WaitEndAnim()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GameManager.Instance.LaunchGame());

    }

    public void OpenMainMenu()
    {
        Helpers.ShowCanva(_canvasGroup);
    }

    public void OpenCredit()
    {
        _animatorTransition.SetTrigger("ClickCredit");
        Helpers.HideCanva(_canvasGroup);
        
    }

    public void OpenLegal() {
        //animator.SetTrigger("Click");
        _animatorTransition.SetTrigger("ClickLegal");
        Helpers.HideCanva(_canvasGroup);
        Helpers.ShowCanva(_canvasGroup);
    }

    public void OpenTrophy()
    {
        _animatorTransition.SetTrigger("ClickTrophy");
        Helpers.HideCanva(_canvasGroup);
    }
}
