using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerSkill playerSkill)
    {
        switch (playerSkill)
        {
            case PlayerSkill.PantsSkill:
                Debug.Log("On m'a fouetté avec un slip");

                break;
        }
    }

}
