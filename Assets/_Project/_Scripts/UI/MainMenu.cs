using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
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

    public void OpenCredit()
    {
        Debug.Log("on va dans les credit");
    }
}
