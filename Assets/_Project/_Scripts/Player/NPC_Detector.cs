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
            SoundSystem.Instance.PlayRandomSoundFXClipByKeys(new string[] { "Boo Curious One", "Boo Curious Two",}, transform.position);
            OnDetectNPC?.Invoke();
            _npcInRange++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDetectable detectable = other.gameObject.GetComponent<IDetectable>();
        if (detectable != null)
        {
            _npcInRange--;

            detectable.NoDetected();

            if (_npcInRange == 0)
                OnStopDetectNPC?.Invoke();

            _encounteredColliders.Remove(other);
        }
    }

    private bool ToggleOutlineWithAnger(Collider other)
    {
        IDetectable detectable = other.gameObject.GetComponent<IDetectable>();
        if (detectable != null)
        {
            if (AngrySystem.Instance != null)
            {
                if (AngrySystem.Instance.IsAngry)
                {

                    detectable.NoDetected();
                   
                }
                else
                {
                    detectable.Detected();
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
            if (other != null)
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
