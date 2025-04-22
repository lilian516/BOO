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
    }

    public void Init(Vector3 dir, Quaternion rotation)
    {
        _forward = dir.normalized;

        Transform child = transform.GetChild(0);

        float threshold = 0.25f;

        if (_forward.x < -1f + threshold && Mathf.Abs(_forward.z) < threshold)
        {
            child.rotation = Quaternion.Euler(35.0f, 0.0f, 0.0f);
        }
        else if (_forward.x > 1f - threshold && Mathf.Abs(_forward.z) < threshold)
        {
            child.rotation = Quaternion.Euler(-35.0f, 180.0f, 0.0f);
        }
        else
        {
            child.rotation = Quaternion.Euler(90.0f, 90.0f + rotation.eulerAngles.y, 0.0f);
        }

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
        }
    }
}

