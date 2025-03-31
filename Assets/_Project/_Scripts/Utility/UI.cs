using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void HideUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }

    public void ShoweUI(GameObject uiElement)
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
    }
}