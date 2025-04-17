using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishman : MonoBehaviour, ISpeakable, IChangeable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Speak()
    {
        DialogueSystem.Instance.BeginDialogue(_basicDialogue);
    }

    public void Change()
    {
        Debug.Log("CHANGE WESH");
        _animator.SetBool("IsBooMad",true);
    }

    public void ResetChange()
    {
        _animator.SetBool("IsBooMad", false);
    }
}
