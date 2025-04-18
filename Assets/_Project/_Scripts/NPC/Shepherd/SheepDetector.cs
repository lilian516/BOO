using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepDetector : MonoBehaviour
{
    [HideInInspector] public int SheepCount;

    private void OnTriggerEnter(Collider other)
    {

        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null)
        {
            sheep.IsGoodPosition = true;
            SheepCount++;
        }
        
            
        
    }

    private void OnTriggerExit(Collider other)
    {
        Sheep sheep = other.GetComponent<Sheep>();
        if (sheep != null)
        {
            sheep.IsGoodPosition = false;
            SheepCount--;
        }
    }
}
