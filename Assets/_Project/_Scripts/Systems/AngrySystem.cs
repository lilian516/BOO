using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AngrySystem : Singleton<AngrySystem>
{
    private int _remainingLives;
    private int _angryLimits;
    private int _baseAngryLimits;
    private int _calmLimits;
    private int _baseCalmLimits;
    public bool IsAngry;
    public GameObject FlamePrefab;
    public GameObject[] Islands;
    List<GameObject> _flamsSpawnPoints;
    public GameObject EnvironmentCapsule;
    private int _amountOfFlames;

    public int AngryLimits { get => _angryLimits; }
    public int CalmLimits { get => _calmLimits; }

    public delegate void ChangeElements();
    public event ChangeElements OnChangeElements;

    public delegate void FirstAngerOccurence();
    public event FirstAngerOccurence OnFirstAngerOccurence;

    public delegate void SecondAngerOccurence();
    public event SecondAngerOccurence OnSecondAngerOccurence;

    public delegate void FirstCalmOccurence();
    public event FirstCalmOccurence OnFirstCalmOccurence;

    public delegate void SecondCalmOccurence();
    public event SecondCalmOccurence OnSecondCalmOccurence;

    public delegate void ResetChangedElements();
    public event ResetChangedElements OnResetElements;



    void Start()
    {
        IsAngry = false;
        _baseAngryLimits = 3;
        _baseCalmLimits = 3;
        _angryLimits = 3;
        _calmLimits = 3;
        _remainingLives = 3;
        _amountOfFlames = 5;

        Islands = new GameObject[3];
    }

    public void ChangeAngryLimits()
    {
        if (_angryLimits > 0)
            _angryLimits--;

        if (_angryLimits == 2)
        {
            OnFirstAngerOccurence?.Invoke();
            return;
        }
        else if (_angryLimits == 1)
        {
            OnSecondAngerOccurence?.Invoke();
            return;
        }

        if (_angryLimits == 0 && !IsAngry)
        {
            SpawnMultipleOnRandomBases();

            _angryLimits = _baseAngryLimits;

            StartCoroutine(WaitForChange());

            OnChangeElements?.Invoke();
            IsAngry = true;
            _angryLimits = _baseAngryLimits;

            VibrationSystem.Instance.VibratePhone(100.0f, 3.0f);
        }
    }

    private IEnumerator WaitForChange()
    {
        InputManager.Instance.Controls.Disable();

        yield return new WaitForSeconds(4);

        InputManager.Instance.Controls.Enable();
    }

    public void ChangeCalmLimits()
    {
        Debug.Log($"[Calm] Calm limit before: {_calmLimits}");
        if (_calmLimits <= 0)
        {
            Debug.Log($"[ChangeCalm] Changement ignoré car limite = {_calmLimits}");
            return;
        }
        _calmLimits--;
        Debug.Log($"[Calm] Calm limit is now: {_calmLimits}");
        if (_calmLimits == 2)
        {
            Debug.Log("[Calm] First calm event");
            OnFirstCalmOccurence?.Invoke();
        }
        else if (_calmLimits == 1)
        {
            Debug.Log("[Calm] Second calm event");
            OnSecondCalmOccurence?.Invoke();
        }

        if (_calmLimits == 0 && IsAngry)
        {
            Debug.Log("[Calm] Reset elements!");

            IsAngry = false;

            StartCoroutine(WaitForChange());

            OnResetElements?.Invoke();
            _calmLimits= _baseCalmLimits;

            _remainingLives--;
        }
    }

    private void Update()
    {
        CheckRemainingLives();
    }

    private void CheckRemainingLives()
    {
        if (_remainingLives <= 0)
        {
            // Faire charger l'�cran de Lose ou reset la game directement mais on a pas encore ce qu'il faut donc petit debug log cadeau la famille

            Debug.Log("You got mad too many times... too bad");
        }
    }


    private void SpawnMultipleOnRandomBases()
    {
        FindFlames();
        
        if (FlamePrefab == null || _flamsSpawnPoints.Count == 0)
        {
            Debug.LogWarning("Missing prefab or base objects.");
            return;
        }

        for (int i = 0; i < _amountOfFlames; i++)
        {
            GameObject baseObj = _flamsSpawnPoints[Random.Range(0, _flamsSpawnPoints.Count)];

            GameObject flame = Instantiate(FlamePrefab, baseObj.transform.position, FlamePrefab.transform.rotation);

            float newScale = Random.Range(0.25f, 1.0f);

            flame.transform.SetParent(baseObj.transform);

            flame.transform.localScale *= newScale;

            flame.transform.position += flame.transform.up * (4 * newScale);

            _flamsSpawnPoints.Remove(baseObj);
        }

        _flamsSpawnPoints.Clear();

    }

    public void FindFlames()
    {
        string GameObjectName = "E_Flam_Spawnpoints_" + (3 - _remainingLives);
        Debug.Log(GameObjectName);
        _flamsSpawnPoints = FindAllObjectWithNameInScene("MainScene", GameObjectName);
    }

    public List<GameObject> FindAllObjectWithNameInScene(string sceneName, string objectName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.Log("Found ENVIRONMENT object: " + EnvironmentCapsule.name);
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' is not loaded or invalid.");

        }

        List<GameObject> list = new List<GameObject>();

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            FindAllInChildrenRecursive(root.transform, objectName, list);
        }
       
        Debug.Log(list.Count);

        foreach (GameObject listd in list)
        {
            listd.name = "cet objet est dans la liste";
        }

        return list;
    }

    GameObject FindGameObjectInScene(string sceneName, string objectName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid() || !scene.isLoaded)
        {
            Debug.LogWarning($"Scene '{sceneName}' is not loaded or invalid.");
            return null;
        }

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform result = FindInChildrenRecursive(root.transform, objectName);
            if (result != null)
                return result.gameObject;
        }

        return null;
    }

    Transform FindInChildrenRecursive(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindInChildrenRecursive(child, name);
            if (result != null)
                return result;
        }

        return null;
    }

    void FindAllInChildrenRecursive(Transform parent, string name, List<GameObject> results)
    {
        if (parent.name == name)
            results.Add(parent.gameObject);

        foreach (Transform child in parent)
        {
            FindAllInChildrenRecursive(child, name, results);
        }
    }
}
