using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem.OnScreen;


[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string DIALOGUE_UI_TAG = "DialogueUI";
    private const string GAME_CONTROLLER_TAG = "UIController";

    private const string SKILL_STICK_PARENT_TAG = "SkillStick";

    private const string INVENTORY_UI_TAG = "InventoryUI";
    private const string BACKGROUND_UI_TAG = "BackgroundUI";
    private const string INVENTORY_FULL_TAG = "InventoryFull";

    private const string ACHIEVEMENT_UI_TAG = "AchievementUI";
    private const string ACHIEVEMENT_LIST_TAG = "AchievementList";


    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public SoundSystem SoundSystem;
    [HideInInspector] public GameObject DialogueUI;
    [HideInInspector] public GameObject InventoryUI;
    [HideInInspector] public GameObject SkillStickParent;

    
    [HideInInspector] public Inventory InventorySkill;
    [HideInInspector] public GameObject GameController;
    [HideInInspector] public GameObject UIBackground;
    [HideInInspector] public GameObject InventoryFullMenu;
    [HideInInspector] public GameObject UIAchievement;
    [HideInInspector] public GameObject UIAchievementButton;
    [HideInInspector] public GameObject UIAchievementMenu;
    [HideInInspector] public GameObject UIAchievementList;

    [HideInInspector] public int KilledSheep;

    private void Start()
    {
        StartCoroutine(WaitForScenesAndInitialize());

        KilledSheep = 0;

    }

    

    private IEnumerator WaitForScenesAndInitialize()
    {
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "CJOLI", "MainMenu" });

        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        InventorySkill = Player.GetComponent<Inventory>();
        SoundSystem.SetAudioListener(MainCamera.GetComponent<AudioListener>());
        SoundSystem.ChangeMusicByKey("Main Music");

        UIAchievementList = GameObject.FindGameObjectWithTag(ACHIEVEMENT_LIST_TAG);
        //InventoryUI = GameObject.FindGameObjectWithTag(INVENTORY_UI_TAG);

        //InventorySkill.Init();
    }

    public IEnumerator LaunchGame()
    {
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "UIInGame"});

        SkillStickParent = GameObject.FindGameObjectWithTag(SKILL_STICK_PARENT_TAG);
        DialogueUI = GameObject.FindGameObjectWithTag(DIALOGUE_UI_TAG);
        InventoryUI = GameObject.FindGameObjectWithTag(INVENTORY_UI_TAG);
        UIBackground = GameObject.FindGameObjectWithTag(BACKGROUND_UI_TAG);
        InventoryFullMenu = GameObject.FindGameObjectWithTag(INVENTORY_FULL_TAG);

        UIAchievement = GameObject.FindGameObjectWithTag(ACHIEVEMENT_UI_TAG);

        DialogueSystem.Instance.Init();
        InventorySkill.Init();

        GameController = GameObject.FindGameObjectWithTag(GAME_CONTROLLER_TAG);

        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "MainMenu" });



        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public GameObject SpawnObject(GameObject obj)
    {
        GameObject objInstance = Instantiate(obj);
        return objInstance;
    }

    public IEnumerator BackToMainMenu()
    {
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainMenu" });
        UIAchievementList = GameObject.FindGameObjectWithTag(ACHIEVEMENT_LIST_TAG);
        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "UIInGame" });
    }
}