using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerInGame : MonoBehaviour, IChangeable
{

    [SerializeField] GameObject _joystickCenter;
    [SerializeField] GameObject _joystickExterior;
    [SerializeField] GameObject _joystickSkill;

    [SerializeField] List<Sprite> _listJoystickSpriteCenter;
    [SerializeField] List<Sprite> _listJoystickSpriteExterior;
    [SerializeField] List<Sprite> _listJoystickSkill;



    void Start()
    {
        AngrySystem.Instance.OnFirstAngerOccurence += UpdateAngryModeFirst;
        AngrySystem.Instance.OnSecondAngerOccurence += UpdateAngryModeSecond;
        AngrySystem.Instance.OnChangeElements += Change;

        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    public void ResetChange()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[0];
        _joystickExterior.GetComponent<Image>().sprite = _listJoystickSpriteExterior[0];
        _joystickSkill.GetComponent<Image>().sprite = _listJoystickSkill[0];
    }

    private void UpdateAngryModeFirst()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[1];
    }

    private void UpdateAngryModeSecond()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[2];
    }


    public void Change()
    {
        _joystickCenter.GetComponent<Image>().sprite = _listJoystickSpriteCenter[3];
        _joystickExterior.GetComponent<Image>().sprite = _listJoystickSpriteExterior[1];
        _joystickSkill.GetComponent<Image>().sprite = _listJoystickSkill[1];
    }

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnFirstAngerOccurence -= UpdateAngryModeFirst;
            AngrySystem.Instance.OnSecondAngerOccurence -= UpdateAngryModeSecond;
            AngrySystem.Instance.OnChangeElements -= Change;

            
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }


}
