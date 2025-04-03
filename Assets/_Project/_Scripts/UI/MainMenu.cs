using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    [SerializeField] Sprite _buttonPlayClicked;
    [SerializeField] Sprite _buttonSettingsClicked;
    [SerializeField] Sprite _buttonCreditClicked;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton(Image image)
    {
        //image.sprite = _buttonPlayClicked;
        image.gameObject.GetComponent<Animator>().SetTrigger("Click");

        StartCoroutine(WaitEndAnim());
        
        Debug.Log("on start le jeu");
    }

    private IEnumerator WaitEndAnim()
    {
        yield return new WaitForSeconds(0.53f);
        StartCoroutine(GameManager.Instance.LaunchGame());

    }

    public void OpenMainMenu()
    {
        Helpers.ShowCanva(_canvasGroup);
    }

    public void OpenCredit(Image image)
    {
        image.gameObject.GetComponent<Animator>().SetTrigger("Click");
        Debug.Log("on va dans les credit");
        
    }

    public void OpenLegal(Animator animator) {
        animator.SetTrigger("Click");
    }
}
