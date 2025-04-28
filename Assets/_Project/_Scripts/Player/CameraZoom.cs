using System.Collections;
using System.Collections.Generic;
using Cinemachine;

using UnityEngine;


public class CameraZoom : MonoBehaviour ,IChangeable
{
    private float _baseFOV;
    private float _currentFOV;
    private float _zoomFOV;

    private void Start()
    {
        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;

        _baseFOV = GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;

        _currentFOV = _baseFOV;
        _zoomFOV = 24;
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

        while (GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView > _zoomFOV || GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y > -0.5)
        {
            if (GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y > -0.5)
                GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y -= (0.3f / elapsedTime) * Time.deltaTime;
            
            if (GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView > _zoomFOV)
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

        while (GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView < _baseFOV || GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y < 0)
        {
            if (GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y < 0)
                GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y += (0.3f / elapsedTime) * Time.deltaTime;
            
            if (GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView < _baseFOV)
                GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView += (15 / elapsedTime) * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements -= Change;
            AngrySystem.Instance.OnResetElements -= ResetChange;
        }
    }
}
