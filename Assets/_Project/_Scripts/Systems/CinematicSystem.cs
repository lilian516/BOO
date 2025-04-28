using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class CinematicSystem : Singleton<CinematicSystem>
{
    private VideoPlayer _videoPlayer;

    private CanvasGroup _cinematicCanvasGroup;

    public delegate void EndCinematicEvent();
    public event EndCinematicEvent OnEndCinematic;

    private bool _isPlaying;

    protected override void Awake()
    {
        base.Awake();
        _isPlaying = false;
        _videoPlayer = GetComponent<VideoPlayer>();
    }
    private void Start()
    {
        _videoPlayer.loopPointReached += StopCinematic;
    }

    private void SetCanvaGroup()
    {
        if (_cinematicCanvasGroup == null)
        {
            _cinematicCanvasGroup = GameObject.Find("Cinematic").GetComponent<CanvasGroup>();
        }
    }
    private void ToggleVideo()
    {
        SetCanvaGroup();

        _isPlaying = !_isPlaying;
        if (_isPlaying)
        {
            _cinematicCanvasGroup.alpha = 1;
            _cinematicCanvasGroup.interactable = true;
            _cinematicCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            _cinematicCanvasGroup.alpha = 0;
            _cinematicCanvasGroup.interactable = false;
            _cinematicCanvasGroup.blocksRaycasts = false;
        }
        
    }
    public void PlayCinematic(string name)
    {
        ToggleVideo();
        _videoPlayer.targetCamera = Camera.main;
        string videoPath = "Videos/" + name;
        VideoClip clip = Resources.Load<VideoClip>(videoPath);
        _videoPlayer.clip = clip;
        _videoPlayer.Play();
    }

    public void StopCinematic( VideoPlayer video) {
        ToggleVideo();
        _videoPlayer.Stop();
        _videoPlayer.targetTexture.Release();
        OnEndCinematic?.Invoke();
    }

    protected override void OnApplicationQuit ()
    {
        _videoPlayer.targetTexture.Release();
    }
}
