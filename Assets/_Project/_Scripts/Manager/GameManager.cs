using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";

    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public SoundSystem SoundSystem;

    private void Start()
    {

        //SceneManager.LoadScene("AssetImplementation", LoadSceneMode.Additive);
        //SceneManager.LoadScene("UIInGame", LoadSceneMode.Additive);
        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        SoundSystem.SetAudioListener(MainCamera.GetComponent<AudioListener>());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //StartCoroutine(WaitForScenesAndInitialize());
    }

    

    private IEnumerator WaitForScenesAndInitialize()
    {
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainScene", "UIInGame" });

        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        SoundSystem.SetAudioListener(MainCamera.GetComponent<AudioListener>());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public GameObject SpawnObject(GameObject obj)
    {
        GameObject objInstance = Instantiate(obj);
        return objInstance;
    }
}