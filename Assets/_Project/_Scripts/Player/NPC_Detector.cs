using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Detector : MonoBehaviour
{
    public delegate void DetectNPC();
    public DetectNPC OnDetectNPC;
    public delegate void StopDetectNPC();
    public StopDetectNPC OnStopDetectNPC;

    private int _npcInRange = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ISpeakable>() != null)
        {
            OnDetectNPC?.Invoke();
            _npcInRange++;

            other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineSize", 3.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<ISpeakable>() != null)
        {
            _npcInRange--;

            other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineSize", 0.0f);

            if (_npcInRange == 0)
                OnStopDetectNPC?.Invoke();
        }
    }

    public void SetDetectorRadius(float radius)
    {
        GetComponent<SphereCollider>().radius = radius;
    }
}
