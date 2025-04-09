using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [HideInInspector] public float RotationStrength;
    [HideInInspector] public float RotationSpeed;
    private float _fanUsage;
    [SerializeField] private float _size;
    // Start is called before the first frame update
    void Start()
    {
        RotationStrength = 1.0f;
        RotationSpeed = 1.0f;

        _fanUsage = Random.Range(1.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        RotateFan();
    }

    private void RotateFan()
    {
        float zRotation = transform.eulerAngles.z; 
        float addedRotation = 50f * Time.deltaTime * RotationStrength * RotationSpeed * (1.0f / _fanUsage) * (1.0f / _size); 

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            zRotation + addedRotation
        );
    }
}
