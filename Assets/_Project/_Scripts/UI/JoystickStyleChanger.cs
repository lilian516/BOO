using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class JoystickStyleChanger : MonoBehaviour
{
    [SerializeField] Sprite[] _sprites = new Sprite[2];
    [SerializeField] RectTransform _imageObject;
    private Image _imageComponent;

    private readonly float _duration = 0.8f;

    [SerializeField] float[] _magnitudes = new float[3];
    private int _magnitudeIndex;

    private bool _isShaking = false;

    void Start()
    {
        
        _imageComponent = _imageObject.GetComponent<Image>();
        
        _imageComponent.sprite = _sprites[0];
        AngrySystem.Instance.OnFirstAngerOccurence += UpdateAngryMode;
        AngrySystem.Instance.OnSecondAngerOccurence += UpdateAngryMode;
        AngrySystem.Instance.OnChangeElements += Change;

        AngrySystem.Instance.OnResetElements += ResetChange;

        if (SaveSystem.Instance != null)
        {
            _magnitudeIndex = SaveSystem.Instance.LoadElement<int>("MagnitudeIndex");
        }
        else
        {
            Debug.LogError("Le système de sauvegarde n'a pas d'instance active.");
        }

    }

    void UpdateAngryMode()
    {
        if (!_isShaking)
        {
            StartCoroutine(Shake());
        }

        if (_magnitudeIndex >= _magnitudes.Length - 1)
        {
            _imageComponent.sprite = _sprites[1];
        }
    }

    private IEnumerator Shake()
    {
        _isShaking = true;

        if (_magnitudeIndex >= _magnitudes.Length)
        {
            _isShaking = false;
            yield break;
        }

        Vector2 originalPos = _imageObject.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < _duration) 
        {
            float offsetX = Random.Range(-1f, 1f) * _magnitudes[_magnitudeIndex];
            float offsetY = Random.Range(-1f, 1f) * _magnitudes[_magnitudeIndex];

            _imageObject.anchoredPosition = originalPos + new Vector2(offsetX, offsetY);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _imageObject.anchoredPosition = originalPos;
        _magnitudeIndex += 1;
        SaveSystem.Instance.SaveElement<int>("MagnitudeIndex", _magnitudeIndex);

        _isShaking = false;
    }

    void UpdateCalmMode()
    {
        StopAllCoroutines();
        _imageComponent.sprite = _sprites[0];
        _magnitudeIndex = 0;
        SaveSystem.Instance.ResetElement<int>("MagnitudeIndex");
        _isShaking = false;
    }

    public void Change()
    {
        UpdateAngryMode();
    }

    public void ResetChange()
    {
        UpdateCalmMode();
    }

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnFirstAngerOccurence -= UpdateAngryMode;
            AngrySystem.Instance.OnSecondAngerOccurence -= UpdateAngryMode;
            AngrySystem.Instance.OnChangeElements -= Change;

            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }
}
