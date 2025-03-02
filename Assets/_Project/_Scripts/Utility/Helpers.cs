using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

