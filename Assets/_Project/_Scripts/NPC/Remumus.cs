using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remumus : MonoBehaviour, ISpeakable
{
    [SerializeField] DialogueAsset _basicDialogue;
    [SerializeField] DialogueAsset _dryUnderwearDialogue;

    public void Speak()
    {
        DialogueSystem.Instance.BeginDialogue(_basicDialogue);
        if (1 == 1)
        {
            DialogueSystem.Instance.BeginDialogue(_dryUnderwearDialogue);
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
}
