using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        foreach (Sprite sprite in _sprites)
        {
            if (sprite == null)
            {
                Debug.LogError("L'un des deux sprites n'est pas impl�ment�, le code ne peut pas continuer.");
                return;
            }
        }
        if (_imageObject.GetComponent<Image>() == null)
        {
            Debug.LogError("Le composant sur lequel est accroch� ce code n'a pas de component Image. Veuillez rev�rifier le code, ce dernier ne peut pas continuer.");
            return;
        }
        else
        {
            _imageComponent = _imageObject.GetComponent<Image>();
        }
        _imageComponent.sprite = _sprites[0];
        AngrySystem.Instance.OnFirstAngerOccurence += UpdateAngryMode;
        AngrySystem.Instance.OnSecondAngerOccurence += UpdateAngryMode;
        AngrySystem.Instance.OnChangeElements += Change;

        AngrySystem.Instance.OnResetElements += ResetChange;

        _magnitudeIndex = 0;

    }

    // Update is called once per frame
    void UpdateAngryMode()
    {
        //Debug.Log($"Magnitude Index en entr�e : {_magnitudeIndex}");
        if (!_isShaking)
        {
            StartCoroutine(Shake());
        }

        if (_magnitudeIndex >= _magnitudes.Length - 1)
        {
            _imageComponent.sprite = _sprites[1];
            Debug.Log($"Sprite applied : {_sprites[1]}");
        }
        Debug.Log($"Magnitude Index en sortie : {_magnitudeIndex}");
    }

    private IEnumerator Shake()
    {
        _isShaking = true;

        if (_magnitudeIndex >= _magnitudes.Length)
        {
            Debug.LogWarning("Shake ignor� : index de magnitude hors limites.");
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

        _isShaking = false;
        Debug.Log("Coroutine finie");
    }

    void UpdateCalmMode()
    {
        StopAllCoroutines();
        _imageComponent.sprite = _sprites[0];
        _magnitudeIndex = 0;
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
