using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSystem : Singleton<LoadSceneSystem>
{
    [SerializeField] private GameObject _loadingObject;
    [SerializeField] CanvasGroup _canvasGroupLoading;
    [SerializeField] private Animator _loadingScreenAnimator;


    private bool _fakeLoading;
    public IEnumerator LoadTargetScenes(string[] targetScenes, bool needLoadingScreen)
    {
        if (needLoadingScreen)
        {
            Helpers.ShowCanva(_canvasGroupLoading);
            _loadingScreenAnimator.SetBool("IsLoading", true);
        }

        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }

        if (!_fakeLoading)
        {
            Helpers.HideCanva(_canvasGroupLoading);
            _loadingScreenAnimator.SetBool("IsLoading", false);
        }

    }
    public IEnumerator UnloadTargetScenes(string[] targetScenes, bool needLoadingScreen)
    {
        if (needLoadingScreen)
        {
            Helpers.ShowCanva(_canvasGroupLoading);
            _loadingScreenAnimator.SetBool("IsLoading", true);
        }

        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.UnloadSceneAsync(scene);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }


        if (!_fakeLoading)
        {
            Helpers.HideCanva(_canvasGroupLoading);
            _loadingScreenAnimator.SetBool("IsLoading", false);
        }
    }

    public IEnumerator FakeLoadingScreen(float duration)
    {
        Helpers.ShowCanva(_canvasGroupLoading);
        _loadingScreenAnimator.SetBool("IsLoading", true);
        _fakeLoading = true;

        yield return new WaitForSeconds(duration);
        Helpers.HideCanva(_canvasGroupLoading);
        _loadingScreenAnimator.SetBool("IsLoading", false);
        _fakeLoading = false;
    }
}
