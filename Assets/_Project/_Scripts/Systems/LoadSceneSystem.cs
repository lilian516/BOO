using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSystem : Singleton<LoadSceneSystem>
{
    [SerializeField] private GameObject _loadingObject;
    [SerializeField] private Animator _loadingScreenAnimator;
    public IEnumerator LoadTargetScenes(string[] targetScenes)
    {
        _loadingObject.SetActive(true);
        _loadingScreenAnimator.SetBool("IsLoading", true);

        yield return new WaitForSeconds(1.0f);

        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }
        _loadingObject.SetActive(false);
        _loadingScreenAnimator.SetBool("IsLoading", false);
    }
    public IEnumerator UnloadTargetScenes(string[] targetScenes)
    {
        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.UnloadSceneAsync(scene);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }
    }
}
