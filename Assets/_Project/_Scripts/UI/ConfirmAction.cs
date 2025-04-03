using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ConfirmationPopup : MonoBehaviour
{
    public GameObject popupUI;
    public Button yesButton;
    public Button noButton;
    private int userChoice = 0;

    private void init()
    {
        yesButton.onClick.AddListener(OnYesPressed);
        noButton.onClick.AddListener(OnNoPressed);
    }

    public IEnumerator WaitForConfirmation()
    {
        userChoice = 0;
        popupUI.SetActive(true);
        Debug.Log("Popup activ�. En attente de confirmation...");
        init();
        while (userChoice == 0)
        {
            if (!popupUI.activeInHierarchy)
            {
                Debug.LogError("popupUI d�sactiv� de mani�re inattendue !");
                yield break;
            }
            Debug.Log($"UserChoice actuel : {userChoice}");
            yield return null;
        }

        Debug.Log($"Confirmation re�ue : UserChoice = {userChoice}");
        popupUI.SetActive(false);
    }

    public bool IsConfirmed()
    {
        return userChoice == 2;
    }

    private void OnYesPressed()
    {
        userChoice = 2;
    }

    private void OnNoPressed()
    {
        userChoice = 1;
    }
}

//Utilisation exemple : 
//#region Init value
//[SerializeField] private GameObject confirmationPopup;
//private ConfirmationPopupManager confirmationPopupManager;
//#endregion

//public void RequestLeavingGame()
//{
//    confirmationPopupManager = confirmationPopup.GetComponent<ConfirmationPopupManager>();
//    StartCoroutine(HandleLeaving());
//}

//private IEnumerator HandleLeaving()
//{
//    yield return confirmationPopupManager.WaitForConfirmation();

//    if (confirmationPopupManager.IsConfirmed())
//    {
//        QuitGame();
//    }
//}

//public void QuitGame()
//{
//    Debug.Log("QuitGame() appel�.");
//    Application.Quit();

//#if UNITY_EDITOR
//    Debug.Log("Arr�t dans l'�diteur.");
//    UnityEditor.EditorApplication.isPlaying = false;
//#endif