using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Detector : MonoBehaviour, IChangeable
{
    public delegate void DetectNPC();
    public DetectNPC OnDetectNPC;
    public delegate void StopDetectNPC();
    public StopDetectNPC OnStopDetectNPC;

    private List<Collider> _encounteredColliders;

    private int _npcInRange = 0;

    private void Start()
    {
        _encounteredColliders = new List<Collider>();

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ToggleOutlineWithAnger(other))
        {
            _encounteredColliders.Add(other);
            OnDetectNPC?.Invoke();
            _npcInRange++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<ISpeakable>() != null || other.gameObject.GetComponent<IClickable>() != null)
        {
            _npcInRange--;

            if (other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null)
                other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineSize", 0.0f);

            if (other.GetComponent<Door>() != null)
                other.GetComponent<Door>().DoorOutlineMaterial.SetFloat("_Outline_Thickness", 0.0f);

            if (_npcInRange == 0)
                OnStopDetectNPC?.Invoke();

            _encounteredColliders.Remove(other);
        }
    }

    private bool ToggleOutlineWithAnger(Collider other)
    {
        if (other.gameObject.GetComponent<ISpeakable>() != null || other.gameObject.GetComponent<IClickable>() != null)
        {
            if (AngrySystem.Instance != null)
            {
                if (AngrySystem.Instance.IsAngry)
                {
                    if (other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null)
                        other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineSize", 0.0f);

                    if (other.GetComponent<Door>() != null)
                        other.GetComponent<Door>().DoorOutlineMaterial.SetFloat("_Outline_Thickness", 0.0f);
                }
                else
                {
                    if (other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null)
                        other.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_OutlineSize", 3.0f);

                    if (other.GetComponent<Door>() != null)
                        other.GetComponent<Door>().DoorOutlineMaterial.SetFloat("_Outline_Thickness", 0.01f);
                }
            }
            return true;
        }

        return false;
    }

    public void Change()
    {
        if (_encounteredColliders.Count <= 0)
            return;

        foreach (Collider other in _encounteredColliders)
        {
            Debug.Log("boobooboo");
            ToggleOutlineWithAnger(other);
        }
    }

    public void ResetChange()
    {
        if (_encounteredColliders.Count <= 0)
            return;

        foreach (Collider other in _encounteredColliders)
        {
            if (other != null)
                ToggleOutlineWithAnger(other);
        }
    }

    public void SetDetectorRadius(float radius)
    {
        GetComponent<SphereCollider>().radius = radius;
    }

}
