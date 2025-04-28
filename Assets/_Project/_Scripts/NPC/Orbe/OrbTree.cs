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
        switch (playerSkill)
        {
            case PlayerSkill.PantsSkill:
                if (rb != null) {
                    rb.isKinematic = false;
                    GetComponent<Animator>().SetTrigger("Touched");
                    StartCoroutine(ReactivateOrb());
                }
                
                break;
        }
    }

    private IEnumerator ReactivateOrb()
    {
        yield return new WaitForSeconds(2.0f);

        rb.isKinematic = true;
    }
}
