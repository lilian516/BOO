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

    private float _duration = 0.8f;

    [SerializeField] float[] _magnitudes = new float[3];
    private int _magnitudeIndex;

    [SerializeField] JoystickAnger _joystickAnger;

    private bool _isShaking = false;

    void Start()
    {
        foreach (Sprite sprite in _sprites)
        {
            if (sprite == null)
            {
                Debug.LogError("L'un des deux sprites n'est pas implémenté, le code ne peut pas continuer.");
                return;
            }
        }
        if (_imageObject.GetComponent<Image>() == null)
        {
            Debug.LogError("Le composant sur lequel est accroché ce code n'a pas de component Image. Veuillez revérifier le code, ce dernier ne peut pas continuer.");
            return;
        }
        else
        {
            _imageComponent = _imageObject.GetComponent<Image>();
        }
        _imageComponent.sprite = _sprites[0];

        if (_joystickAnger != null)
        {
            _joystickAnger.OnChangeAngerLevel += UpdateAngryMode;
            
        }
        else
        {
            Debug.LogError("JoystickAnger non assigné dans JoystickStyleChanger.");
        }
        //AngrySystem.Instance.OnChangeElements += UpdateAngryMode;
        AngrySystem.Instance.OnResetElements += UpdateCalmMode;

        _magnitudeIndex = 0;

    }

    // Update is called once per frame
    void UpdateAngryMode()
    {
        Debug.Log($"Magnitude Index en entrée : {_magnitudeIndex}");
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
        bool justGotCalmed = false;

        if (_magnitudeIndex >= _magnitudes.Length)
        {
            Debug.LogWarning("Shake ignoré : index de magnitude hors limites.");
            _isShaking = false;
            yield break;
        }
        else if (_magnitudeIndex < 0)
        {
            justGotCalmed = true;
        }
        Vector2 originalPos = _imageObject.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < _duration) 
        {
            float offsetX = Random.Range(-1f, 1f) * _magnitudes[justGotCalmed ? _magnitudeIndex + 1 : _magnitudeIndex];
            float offsetY = Random.Range(-1f, 1f) * _magnitudes[justGotCalmed ? _magnitudeIndex + 1 : _magnitudeIndex];

            _imageObject.anchoredPosition = originalPos + new Vector2(offsetX, offsetY);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _imageObject.anchoredPosition = originalPos;
        if (justGotCalmed)
        {
            _magnitudeIndex = 0;
        }
        else
        {
            _magnitudeIndex += 1;
        }
        _isShaking = false;
        Debug.Log("Coroutine finie");
    }

    void UpdateCalmMode()
    {
        StopAllCoroutines();
        _imageComponent.sprite = _sprites[0];
        _magnitudeIndex = -1;
        _isShaking = false;
    }
}
