using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remumus : MonoBehaviour, ISpeakable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] DialogueAsset _dryUnderwearDialogue;
    [SerializeField] bool _dryUnderwear = false;

    public void Speak()
    {
        if (!_dryUnderwear)
        {
            DialogueSystem.Instance.BeginDialogue(_basicDialogue);
        }
        else
        {
            DialogueSystem.Instance.BeginDialogue(_dryUnderwearDialogue);
            DialogueSystem.Instance.OnEndDialogue += OnEventEndDialogue;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEventEndDialogue()
    {
        if (_dryUnderwear)
        {
            _dryUnderwearDialogue = _dryUnderwearDialogue.NextDialogue;
        }
    }
}
