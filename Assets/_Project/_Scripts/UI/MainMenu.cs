using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    [SerializeField] Sprite _buttonPlayClicked;
    [SerializeField] Sprite _buttonSettingsClicked;
    [SerializeField] Sprite _buttonCreditClicked;
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
        
        Debug.Log("on start le jeu");
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

    public void OpenCredit(Image image)
    {
        _animatorTransition.SetTrigger("ClickCredit");
        Helpers.HideCanva(_canvasGroup);
        //image.gameObject.GetComponent<Animator>().SetTrigger("Click");
        Debug.Log("on va dans les credit");
        
    }

    public void OpenLegal(Animator animator) {
        //animator.SetTrigger("Click");
        _animatorTransition.SetTrigger("ClickLegal");
        Helpers.HideCanva(_canvasGroup);
    }

    public void OpenTrophy()
    {
        _animatorTransition.SetTrigger("ClickTrophy");
        Helpers.HideCanva(_canvasGroup);
    }
}
