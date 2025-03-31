using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    private Vector3 _forward;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _timeMovement;
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
        float elapsedTime = 0.1f;

        while (elapsedTime < _timeMovement)
        {
            transform.Translate(_forward * (_maxSpeed / elapsedTime) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        Destroy(gameObject,2f);
    }

    


    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(PlayerSkill.BubbleSkill);
            Destroy(gameObject);
        }
    }
}
