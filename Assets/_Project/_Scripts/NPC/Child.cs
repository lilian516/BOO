using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour, ISpeakable
{

    [SerializeField] DialogueAsset _dialogue;

    public void Speak()
    {
        DialogueSystem.Instance.BeginDialogue(_dialogue);
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
