using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSystem : Singleton<LoadSceneSystem>
{
    public IEnumerator LoadTargetScenes(string[] targetScenes)
    {
        foreach (string scene in targetScenes)
        {
            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            while (!sceneOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitUntil(() => sceneOperation.isDone);
        }
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
