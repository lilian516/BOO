using System.Collections;

using UnityEngine;

public class Wind : MonoBehaviour
{
    private Vector3 _forward;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        _forward = GameManager.Instance.Player.GetComponent<Player>().LookDir.normalized;


        if (_forward.x < 0)
            transform.GetChild(0).eulerAngles =  new Vector3(35,0,0);

        StartCoroutine(WindMovement());
    }

    IEnumerator WindMovement()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _lifeTime)
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
            interactable.Interact(PlayerSkill.WindSkill);

            if (other.GetComponent<Sheep>() != null)
            {
                other.GetComponent<Sheep>().PushedDirection = _forward;
            }

            //Destroy(gameObject);
        }
    }
}
