using UnityEngine;

public class Inventory : MonoBehaviour
{

    private bool _isPressing;
    private float _timer = 0f;
    private float _pressTime = 1f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (InputManager.Instance.Controls.Player.UseSkill.ReadValue<float>() > 0) // Vérifie si l'utilisateur appuie sur le bouton
        {
            if (!_isPressing)
            {
                _isPressing = true;
                _timer = 0f;
                
                //skillMenu.SetActive(false); // Ferme le menu au début
            }
            else
            {
                _timer += Time.deltaTime;

                // Si la pression dure plus longtemps que le seuil (par exemple 1 seconde), on affiche le menu
                if (_timer >= _pressTime)  // && !skillMenu.activeSelf
                {
                    Debug.Log("on ouvre le menu");
                    //OpenSkillMenu();
                }
            }
        }
        else
        {
            if (_isPressing)
            {
                // Si la pression a duré moins de 1 seconde, exécute la compétence par défaut
                if (_timer < _pressTime)
                {
                    //ExecuteSkill();
                }

                _isPressing = false; // Réinitialiser l'état de pression
            }
        }
    }
}
