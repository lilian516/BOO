using UnityEngine;
using Cinemachine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")]
public class LockXCameraPosition : CinemachineExtension
{
    public bool IsLocked = false;
    public float LockedX = 0f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (IsLocked && stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = LockedX;
            state.RawPosition = pos;
        }
    }
}
