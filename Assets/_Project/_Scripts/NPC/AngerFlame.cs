using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AngerFlame : MonoBehaviour, IChangeable
{
    private float _maxScale;
    private float _minScale;
    void Awake()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        _maxScale = 0.2f;
        _minScale = 0.0f;
    }

    public void Change()
    {
        transform.localScale = new Vector3(
            _maxScale,
            _maxScale,
            _maxScale
            );

        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void ResetChange()
    {
        transform.localScale = new Vector3(
            _minScale,
            _minScale,
            _minScale
            );
    }

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements -= Change;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }
}
