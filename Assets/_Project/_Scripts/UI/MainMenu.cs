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
        image.sprite = _buttonPlayClicked;
        StartCoroutine(GameManager.Instance.LaunchGame());
        Debug.Log("on start le jeu");
    }

    public void CloseMainMenu()
    {

        Helpers.HideCanva(_canvasGroup);
       
    }

    public void OpenMainMenu()
    {
        Helpers.ShowCanva(_canvasGroup);
        
    }

    public void OpenCredit(Image image)
    {
        image.sprite = _buttonCreditClicked;
        Debug.Log("on va dans les credit");
        
    }
}
