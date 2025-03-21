using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    private Vector3 _forward;
    [SerializeField] float _speed;
    // Start is called before the first frame update
    void Start()
    {
        _forward = GameManager.Instance.Player.transform.forward;

        StartCoroutine(BubbleMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BubbleMovement()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            transform.Translate(_forward * _speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

       
        if (interactable != null)
        {
            Debug.Log("L'objet a l'interface IInteractable.");
            
            interactable.Interact( PlayerSkill.BubbleSkill);
        }
    }
}
