using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTree : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody rb;
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
        Debug.Log("On m'a fouetté");
        switch (playerSkill)
        {
            case PlayerSkill.PantsSkill:
                Debug.Log("On m'a fouetté avec un slip");
                rb.isKinematic = false;
                break;
        }
    }

}
