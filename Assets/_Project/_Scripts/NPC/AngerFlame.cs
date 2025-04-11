using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AngerFlame : MonoBehaviour, IChangeable
{
    private bool _isGrowing;
    private bool _isShrinking;
    private float _maxScale;
    private float _minScale;
    // Start is called before the first frame update
    void Awake()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }
    void Start()
    {
        _isGrowing = true;
        _isShrinking = false;
        _maxScale = 0.5f;
        _minScale = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrowing && transform.localScale.x < _maxScale)
        {
            transform.localScale = new Vector3(
                transform.localScale.x + Time.deltaTime * 20.0f,
                transform.localScale.y + Time.deltaTime * 20.0f,
                transform.localScale.z + Time.deltaTime * 20.0f
            );

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + Time.deltaTime * 65.0f,
                transform.position.z
            );

            if (transform.localScale.x >= _maxScale)
            {
                _isGrowing = false;
            }
        }

        if (_isShrinking && transform.localScale.x > _minScale)
        {
            transform.localScale = new Vector3(
                transform.localScale.x - Time.deltaTime * 20.0f,
                transform.localScale.y - Time.deltaTime * 20.0f,
                transform.localScale.z - Time.deltaTime * 20.0f
            );

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - Time.deltaTime * 65.0f,
                transform.position.z
            );

            if (transform.localScale.x <= _minScale)
            {
                _isShrinking = false;
            }
        }
    }

    public void Change()
    {
        _isGrowing = true;
    }

    public void ResetChange()
    {
        _isShrinking = true;
    }
}
