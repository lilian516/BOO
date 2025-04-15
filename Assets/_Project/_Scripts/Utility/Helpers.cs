using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum PlayerSkill
{
    None,
    BubbleSkill,
    WindSkill,
    StickSkill,
    PantsSkill,
    SmashSkill
}

[System.Serializable]
public class SkillDescriptor
{
    public GameObject Prefab;
    public Sprite Sprite;
    public string Name;
    public string Desc;
    public AnimationClip AnimationSkill;
    public float Radius;
    public LayerMask Mask;
}
public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera {
        get {
            if(_camera == null)
                _camera = Camera.main;
            return _camera;
        }
    }

    private static Dictionary<float, WaitForSeconds> _waitDictionay = new();
    public static WaitForSeconds GetWait(float time)
    {
        if (_waitDictionay.TryGetValue(time, out WaitForSeconds wait))
            return wait;

        _waitDictionay[time] = new WaitForSeconds(time);
        return _waitDictionay[time];
    }


    public static void ShowCanva(CanvasGroup canva)
    {
        canva.alpha = 1f;
        canva.interactable = true;
        canva.blocksRaycasts = true;
    }

    public static void HideCanva(CanvasGroup canva)
    {
        canva.alpha = 0f;
        canva.interactable = false;
        canva.blocksRaycasts = false;
    }
}

