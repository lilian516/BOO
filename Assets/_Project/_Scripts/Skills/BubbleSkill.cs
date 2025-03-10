using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSkill : Skill
{
    public Material BubbleMaterial;

    public GameObject BubblePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void UseSkill()
    {
        /* Cette fonction devra remplacer la cr�ation manuelle d'un plane et d'ajouter sa texture 
         * par l'instanciation d'un GameObject Type "Bubble" poss�dant les propri�t�s correspondantes.
         * De m�me, la position de spawn devra �tre chang�e et la direction donn�e � la bulle ajout�e 
         * donnant le forward vector du joueur.
        */
        Vector3 BubbleSpawnPos = Vector3.zero; // � remplacer avec la position du joueur

        //Instantiate(BubblePrefab);

        GameObject BubblePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        BubblePlane.transform.position = BubbleSpawnPos;
        BubblePlane.transform.localScale = Vector3.one;

        BubblePlane.GetComponent<Renderer>().material = BubbleMaterial;
    }
}
