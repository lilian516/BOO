using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishman : MonoBehaviour, ISpeakable
{
    [SerializeField] DialogueAsset _basicDialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Speak()
    {
        DialogueSystem.Instance.BeginDialogue(_basicDialogue);
    }
}
