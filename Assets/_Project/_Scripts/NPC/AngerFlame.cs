using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AngerFlame : MonoBehaviour, IChangeable
{
    private float _maxScale;
    private float _minScale;
    // Start is called before the first frame update
    void Awake()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        _maxScale = 0.5f;
        _minScale = 0.1f;
    }

    public void Change()
    {
        transform.localScale = new Vector3(
            _maxScale,
            _maxScale,
            _maxScale
            );

        transform.localPosition = new Vector3(0.0f, 1.6f, 0.0f);
    }

    public void ResetChange()
    {
        transform.localScale = new Vector3(
            _minScale,
            _minScale,
            _minScale
            );

        transform.localPosition = new Vector3(0.0f, 0.16f, 0.0f);
    }
}
