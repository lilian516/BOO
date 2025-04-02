using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationSystem : Singleton<VibrationSystem>
{
    private bool _isToggled;

    // Start is called before the first frame update
    void Start()
    {
        _isToggled = true;
    }

    public void ToggleVibration()
    {
        _isToggled = !_isToggled;
    }

    public void TriggerVibration(float intensity, float vibrationTime)
    {
        if (!_isToggled) return;
#if UNITY_ANDROID
        VibratePhone(intensity, vibrationTime);

#endif

    }

    private void VibratePhone(float intensity, float vibrationTime)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            long milliseconds = (long)(vibrationTime * 1000);
            int amplitude = Mathf.Clamp((int)(intensity * 255), 0, 255);

            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
            {
                if (vibrator != null)
                {
                    using (AndroidJavaClass vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect"))
                    {
                        AndroidJavaObject effect = vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
                        vibrator.Call("vibrate", effect);
                    }
                }
            }
        }
    }
}
