using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepDetector : MonoBehaviour
{
    [HideInInspector] public int SheepCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Sheep>())
        {
            SheepCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Sheep>())
        {
            SheepCount--;
        }
    }
}
