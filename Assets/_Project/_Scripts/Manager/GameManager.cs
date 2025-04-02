using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using TMPro;


[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string DIALOGUE_UI_TAG = "DialogueUI";

    private const string SKILL_STICK_UI_TAG = "SkillStickUI";

    private const string INVENTORY_UI_TAG = "InventoryUI";


    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public SoundSystem SoundSystem;
    [HideInInspector] public GameObject DialogueUI;
    [HideInInspector] public GameObject InventoryUI;
    [HideInInspector] public GameObject SkillStickUI;
    [HideInInspector] public Inventory InventorySkill;


    private void Start()
    {


        //MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        //SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        //Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        //InventorySkill = Player.GetComponent<Inventory>();
        //SoundSystem.SetAudioListener(MainCamera.GetComponent<AudioListener>());
        //SkillStickUI = GameObject.FindGameObjectWithTag(SKILL_STICK_UI_TAG);

        //DialogueUI = GameObject.FindGameObjectWithTag(DIALOGUE_UI_TAG);
        //InventoryUI = GameObject.FindGameObjectWithTag(INVENTORY_UI_TAG);
        //DialogueSystem.Instance.Init();
        //InventorySkill.Init();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        StartCoroutine(WaitForScenesAndInitialize());
    }

    

    private IEnumerator WaitForScenesAndInitialize()
    {
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainScene", "MainMenu" });

        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        InventorySkill = Player.GetComponent<Inventory>();
        SoundSystem.SetAudioListener(MainCamera.GetComponent<AudioListener>());
        
        //DialogueUI = GameObject.FindGameObjectWithTag(DIALOGUE_UI_TAG);

        //InventoryUI = GameObject.FindGameObjectWithTag(INVENTORY_UI_TAG);
        
        //InventorySkill.Init();
        
    }

    public IEnumerator LaunchGame()
    {
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "UIInGame"});

        SkillStickUI = GameObject.FindGameObjectWithTag(SKILL_STICK_UI_TAG);
        DialogueUI = GameObject.FindGameObjectWithTag(DIALOGUE_UI_TAG);
        InventoryUI = GameObject.FindGameObjectWithTag(INVENTORY_UI_TAG);
        DialogueSystem.Instance.Init();
        InventorySkill.Init();

        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "MainMenu" });


        
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public GameObject SpawnObject(GameObject obj)
    {
        GameObject objInstance = Instantiate(obj);
        return objInstance;
    }
}