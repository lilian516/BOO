using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerInGame : MonoBehaviour, IChangeable
{

    [SerializeField] GameObject _joystickCenter;
    [SerializeField] GameObject _joystickExterior;
    [SerializeField] GameObject _joystickSkill;
    [SerializeField] GameObject _joystickSkillExterior;
    [SerializeField] GameObject _joystickDirectionSmall;
    [SerializeField] GameObject _joystickDirectionBig;

    [SerializeField] List<Sprite> _listJoystickSpriteCenter;
    [SerializeField] List<Sprite> _listJoystickSpriteExterior;
    [SerializeField] List<Sprite> _listJoystickSkill;
    [SerializeField] List<Sprite> _listJoystickSkillExterior;
    [SerializeField] List<Sprite> _listJoystickDirectionSmall;
    [SerializeField] List<Sprite> _listJoystickDirectionBig;



    void Start()
    {
        AngrySystem.Instance.OnFirstAngerOccurence += UpdateAngryModeFirst;
        AngrySystem.Instance.OnSecondAngerOccurence += UpdateAngryModeSecond;
        AngrySystem.Instance.OnFirstCalmOccurence += UpdateAngryModeSecond;
        AngrySystem.Instance.OnSecondCalmOccurence += UpdateAngryModeFirst;
        AngrySystem.Instance.OnChangeElements += Change;

        AngrySystem.Instance.OnResetElements += ResetChange;

        if (AngrySystem.Instance.IsAngry)
        {
            Change();
            switch (AngrySystem.Instance.CalmLimits)
            {
                

                case 2:
                    UpdateAngryModeSecond();
                    break;
                case 1:
                    
                    UpdateAngryModeFirst();
                    break;


            }
            
        }
        else
        {
            switch (AngrySystem.Instance.AngryLimits)
            {
                case 3:
                    ResetChange();
                    break;

                case 2:
                    UpdateAngryModeFirst();
                    break;
                case 1:
                    UpdateAngryModeSecond();
                    break;


            }
        }
    }

    public void ResetChange()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[0];
        _joystickExterior.GetComponent<Image>().sprite = _listJoystickSpriteExterior[0];
        _joystickSkill.GetComponent<Image>().sprite = _listJoystickSkill[0];
        _joystickSkillExterior.GetComponent<Image>().sprite = _listJoystickSkillExterior[0];
        _joystickDirectionSmall.GetComponent<Image>().sprite = _listJoystickDirectionSmall[0];
        _joystickDirectionBig.GetComponent<Image>().sprite = _listJoystickDirectionBig[0];
    }

    private void UpdateAngryModeFirst()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[1];
        StartCoroutine(Shake());
    }

    private void UpdateAngryModeSecond()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[2];
        StartCoroutine(Shake());
    }


    public void Change()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[3];
        _joystickExterior.GetComponent<Image>().sprite = _listJoystickSpriteExterior[1];
        _joystickSkill.GetComponent<Image>().sprite = _listJoystickSkill[1];
        _joystickSkillExterior.GetComponent<Image>().sprite = _listJoystickSkillExterior[1];
        _joystickDirectionSmall.GetComponent<Image>().sprite = _listJoystickDirectionSmall[1];
    
        _joystickDirectionBig.GetComponent<Image>().sprite = _listJoystickDirectionBig[1];
        StartCoroutine(Shake());
    }

    void Update()
    {
        
    }


    private IEnumerator Shake()
    {

        Vector2 originalPos = _joystickExterior.GetComponent<RectTransform>().anchoredPosition;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            float offsetX = Random.Range(-1f, 1f) * 10;
            float offsetY = Random.Range(-1f, 1f) * 10;

            _joystickExterior.GetComponent<RectTransform>().anchoredPosition = originalPos + new Vector2(offsetX, offsetY);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _joystickExterior.GetComponent<RectTransform>().anchoredPosition = originalPos;
       

        
    }

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnFirstAngerOccurence -= UpdateAngryModeFirst;
            AngrySystem.Instance.OnSecondAngerOccurence -= UpdateAngryModeSecond;
            AngrySystem.Instance.OnFirstCalmOccurence -= UpdateAngryModeSecond;
            AngrySystem.Instance.OnSecondCalmOccurence -= UpdateAngryModeFirst;
            AngrySystem.Instance.OnChangeElements -= Change;

            
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }


}
