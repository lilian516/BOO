using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CameraZoom : MonoBehaviour ,IChangeable
{
    private float _baseFOV;
    private float _currentFOV;

    private void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        _baseFOV = GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        _currentFOV = _baseFOV;
    }

    public void Change()
    {
        StartCoroutine(Zoom());
    }

    public void ResetChange()
    {
        StartCoroutine(Zoom());
    }

    private IEnumerator Zoom()
    {
        float elapsedTime = 0.1f;

        while (elapsedTime < 1)
        {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= (15 / elapsedTime) * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(WaitForZoom());
    }

    private IEnumerator WaitForZoom()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomOut()
    {
        float elapsedTime = 0.1f;

        while (elapsedTime < 1)
        {
            GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView += (15 / elapsedTime) * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
