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
        
        if (InputManager.Instance.Controls.Player.UseSkill.ReadValue<float>() > 0) // V�rifie si l'utilisateur appuie sur le bouton
        {
            if (!_isPressing)
            {
                _isPressing = true;
                _timer = 0f;
                
                //skillMenu.SetActive(false); // Ferme le menu au d�but
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
                // Si la pression a dur� moins de 1 seconde, ex�cute la comp�tence par d�faut
                if (_timer < _pressTime)
                {
                    //ExecuteSkill();
                }

                _isPressing = false; // R�initialiser l'�tat de pression
            }
        }
    }
}
