using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickStyleChanger : MonoBehaviour
{
    [SerializeField] Sprite[] _sprites = new Sprite[2];
    [SerializeField] RectTransform _imageObject;
    private Image _imageComponent;
    [SerializeField] bool _toggleUIVibrationAnim;
    private float _duration = 0.8f;
    [SerializeField] float _magnitude = 10f;
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
        AngrySystem.Instance.OnChangeElements += UpdateAngryMode;
        AngrySystem.Instance.OnResetElements += UpdateCalmMode;
    }

    // Update is called once per frame
    void UpdateAngryMode()
    {
        if (_toggleUIVibrationAnim)
        {
            StartCoroutine(Shake());
        }
        _imageComponent.sprite = _sprites[1];
    }

    private IEnumerator Shake()
    {
        Vector2 originalPos = _imageComponent.transform.position;
        float elapsed = 0f;

        while (elapsed < _duration) 
        {
            float offsetX = Random.Range(-1, 1) * _magnitude;
            float offsetY = Random.Range(-1, 1) * _magnitude;

            _imageComponent.transform.position = originalPos + new Vector2(offsetX, offsetY);

            elapsed += Time.deltaTime;
            yield return null;
        }

    }

    void UpdateCalmMode()
    {
        _imageComponent.sprite = _sprites[0];
    }
}
