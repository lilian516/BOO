using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem.OnScreen;
using Cinemachine;


[DefaultExecutionOrder(-10)]
public class GameManager : Singleton<GameManager>
{
    private const string PLAYER_TAG = "Player";
    private const string SOUND_MANAGER_TAG = "SoundManager";
    private const string MAIN_CAMERA_TAG = "MainCamera";
    private const string MAIN_VIRTUAL_CAMERA_TAG = "MainVirtualCamera";
    private const string DIALOGUE_UI_TAG = "DialogueUI";
    private const string DIALOGUE_SKILL_BTN_TAG = "DialogueSkillButton";
    private const string DIALOGUE_QUIT_BTN_TAG = "DialogueQuitButton";
    private const string GAME_CONTROLLER_TAG = "UIController";

    private const string SKILL_STICK_PARENT_TAG = "SkillStick";
    private const string SKILL_STICK_UI_TAG = "SkillStickUI";

    private const string INVENTORY_UI_TAG = "InventoryUI";
    private const string INVENTORY_FULL_TAG = "InventoryFull";

    private const string ACHIEVEMENT_UI_TAG = "AchievementUI";
    private const string ACHIEVEMENT_LIST_TAG = "AchievementList";

    private const string BLACKSCREEN_UI_TAG = "BlackScreen";
    private const string RESET_BUTTON_TAG = "ResetBtn";


    [HideInInspector] public GameObject MainCamera;
    [HideInInspector] public GameObject MainVirtualCamera;
    [HideInInspector] public GameObject Player;
    [HideInInspector] public SoundSystem SoundSystem;
    [HideInInspector] public GameObject DialogueUI;
    [HideInInspector] public GameObject DialogueSkillBtn;
    [HideInInspector] public GameObject DialogueQuitBtn;
    [HideInInspector] public GameObject InventoryUI;
    [HideInInspector] public GameObject SkillStickParent;
    [HideInInspector] public GameObject SkillStickUI;

    
    [HideInInspector] public Inventory InventorySkill;
    [HideInInspector] public GameObject GameController;
    [HideInInspector] public GameObject InventoryFullMenu;
    [HideInInspector] public GameObject UIAchievement;
    [HideInInspector] public GameObject UIAchievementButton;
    [HideInInspector] public GameObject UIAchievementMenu;
    [HideInInspector] public GameObject UIAchievementList;
    [HideInInspector] public GameObject UIBlackscreen;
    [HideInInspector] public GameObject ResetButton;

    [HideInInspector] public int KilledSheep;
    [HideInInspector] public int KilledFly;

    private bool _doIntro;
    private void Start()
    {
        _doIntro = true;
        
        //Application.targetFrameRate = 45;
        KilledSheep = 0;
        KilledFly = 0;

        StartCoroutine(WaitForScenesAndInitialize());
    }

    private IEnumerator WaitForScenesAndInitialize()
    {

        SaveSystem.Instance.Init();
        VibrationSystem.Instance.Init();
        AchievementSystem.Instance.Init();
        AngrySystem.Instance.Init();

        _doIntro = true;

        Application.targetFrameRate = 45;
        KilledSheep = 0;

        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainScene"}, true);
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainMenu" }, true);

        ResetButton = GameObject.FindGameObjectWithTag(RESET_BUTTON_TAG);
        ResetButton.GetComponent<Button>().onClick.AddListener(ResetGame);

        if (_doIntro)
        {
            SoundSystem.Instance.PlaySoundFXClipByKey("Intro Boo Crash");
            yield return LoadSceneSystem.Instance.FakeLoadingScreen(3.1f);
        }
        else
        {
            yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainMenu" }, true);
        }

        MainCamera = GameObject.FindGameObjectWithTag(MAIN_CAMERA_TAG);
        MainVirtualCamera = GameObject.FindGameObjectWithTag(MAIN_VIRTUAL_CAMERA_TAG);
        SoundSystem = GameObject.FindGameObjectWithTag(SOUND_MANAGER_TAG).GetComponent<SoundSystem>();
        Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        InventorySkill = Player.GetComponent<Inventory>();
        SoundSystem.SetAudioListener(MainCamera.GetComponent<AudioListener>());
        SoundSystem.ChangeMusicByKey("Chill Music");

        UIAchievementList = GameObject.FindGameObjectWithTag(ACHIEVEMENT_LIST_TAG);

        Player.GetComponent<Player>().PlayerAnimator.enabled = true;
    }
    public IEnumerator LaunchGame()
    {

        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "UIInGame"}, false);

        SkillStickParent = GameObject.FindGameObjectWithTag(SKILL_STICK_PARENT_TAG);
        SkillStickUI = GameObject.FindGameObjectWithTag(SKILL_STICK_UI_TAG);
        DialogueUI = GameObject.FindGameObjectWithTag(DIALOGUE_UI_TAG);
        DialogueSkillBtn = GameObject.FindGameObjectWithTag(DIALOGUE_SKILL_BTN_TAG);
        DialogueQuitBtn = GameObject.FindGameObjectWithTag(DIALOGUE_QUIT_BTN_TAG);
        InventoryUI = GameObject.FindGameObjectWithTag(INVENTORY_UI_TAG);
        InventoryFullMenu = GameObject.FindGameObjectWithTag(INVENTORY_FULL_TAG);
        UIAchievement = GameObject.FindGameObjectWithTag(ACHIEVEMENT_UI_TAG);
        UIBlackscreen = GameObject.FindGameObjectWithTag(BLACKSCREEN_UI_TAG);

        DialogueSystem.Instance.Init();
        InventorySkill.Init();

        GameController = GameObject.FindGameObjectWithTag(GAME_CONTROLLER_TAG);

        if(!_doIntro)
            InputManager.Instance.EnableControllerSticks();

        if (_doIntro)
            StartCoroutine(WaitForIntro());

        MainVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 2;

        ResetButton.GetComponent<Button>().onClick.RemoveAllListeners();

        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "MainMenu" }, false);

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
        SaveSystem.Instance.SaveAllData();
        yield return LoadSceneSystem.Instance.LoadTargetScenes(new string[] { "MainMenu" }, false);

        ResetButton = GameObject.FindGameObjectWithTag(RESET_BUTTON_TAG);
        ResetButton.GetComponent<Button>().onClick.AddListener(ResetGame);

        UIAchievementList = GameObject.FindGameObjectWithTag(ACHIEVEMENT_LIST_TAG);
        MainVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;

        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "UIInGame" }, false);
    }

    public IEnumerator WaitForIntro()
    {
        _doIntro = false;
        Player.GetComponent<Player>().StartAnim();
        InputManager.Instance.DisableControllerStick();

        yield return new WaitForSeconds(1);

        InputManager.Instance.EnableControllerSticks();

    }

    private void ResetGame()
    {
        Debug.Log("Reset");
        StartCoroutine(UnloadAll());
    }

    private IEnumerator UnloadAll()
    {
        SaveSystem.Instance.ResetAllData();
        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "MainScene", "MainMenu" }, true);
        yield return WaitForScenesAndInitialize();
    }

    [ContextMenu("Lose")]
    public void GameOver()
    {
        InputManager.Instance.DisableControllerStick();
        InputManager.Instance.DisableSkillStick();

        GameObject gameOverScreen = GameObject.FindGameObjectWithTag("GameOverScreen");
        UIBlackscreen = GameObject.FindGameObjectWithTag(BLACKSCREEN_UI_TAG);
        gameOverScreen.GetComponentInChildren<Animator>().SetTrigger("GameOver");
        gameOverScreen.GetComponentInChildren<Button>().onClick.AddListener(delegate { StartCoroutine(Restart()); });

        Helpers.ShowCanva(UIBlackscreen.GetComponent<CanvasGroup>());
        Helpers.ShowCanva(gameOverScreen.GetComponent<CanvasGroup>());
    }

    private IEnumerator Restart()
    {
        yield return LoadSceneSystem.Instance.UnloadTargetScenes(new string[] { "MainScene", "UIInGame" }, true);
        yield return WaitForScenesAndInitialize();
    }
}