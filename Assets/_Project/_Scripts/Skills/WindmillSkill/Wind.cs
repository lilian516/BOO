using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private Vector3 _forward;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        _forward = GameManager.Instance.Player.transform.forward;

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
}
