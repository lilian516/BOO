using System.Collections;
using System.Collections.Generic;
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
    public GameObject EnvironmentCapsule;
    private int _amountOfFlames;

    public int AngryLimits { get => _angryLimits; }
    public int CalmLimits { get => _calmLimits; }

    public delegate void ChangeElements();
    public event ChangeElements OnChangeElements;

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

        // StartCoroutine(FindEnvironmentObject());

        StartCoroutine(FindFlamSpawnpoints());
    }
    [ContextMenu("Change Angry Limits")]
    public void ChangeAngryLimits()
    {
        if (_angryLimits > 0)
            _angryLimits--;

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

    [ContextMenu("Reset Calm Limits")]
    public void ChangeCalmLimits()
    {
        if (_calmLimits > 0)
            _calmLimits--;

        if (_calmLimits == 0 && IsAngry)
        {
            _calmLimits = _baseCalmLimits;

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
            // Faire charger l'écran de Lose ou reset la game directement mais on a pas encore ce qu'il faut donc petit debug log cadeau la famille

            Debug.Log("You got mad too many times... too bad");
        }
    }

    private void SpawnMultipleOnRandomBases()
    {
        if (FlamePrefab == null || Islands.Length == 0)
        {
            Debug.LogWarning("Missing prefab or base objects.");
            return;
        }

        for (int i = 0; i < _amountOfFlames; i++)
        {
            GameObject baseObj = Islands[Random.Range(0, Islands.Length)];
            Renderer baseRenderer = baseObj.GetComponent<Renderer>();

            if (baseRenderer == null)
            {
                Debug.LogWarning("Base object missing Renderer.");
                continue;
            }

            Bounds baseBounds = baseRenderer.bounds;

            float randomX = Random.Range(baseBounds.min.x, baseBounds.max.x);
            float randomZ = Random.Range(baseBounds.min.z, baseBounds.max.z);

            float spawnY = baseBounds.max.y + 0.4974165f;

            Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ);

            GameObject flame = Instantiate(FlamePrefab, spawnPos, FlamePrefab.transform.rotation);

            float newScale = Random.Range(0.25f, 1f);



            SceneManager.MoveGameObjectToScene(flame, EnvironmentCapsule.scene);

            flame.transform.SetParent(EnvironmentCapsule.transform, true);
        }
    }



    IEnumerator FindEnvironmentObject()
    {
        yield return new WaitForSeconds(2.0f);

        EnvironmentCapsule = FindGameObjectInScene("CJOLI", "---------- ENVIRONMENT ----------");
        if (EnvironmentCapsule != null)
        {
            Debug.Log("Found ENVIRONMENT object: " + EnvironmentCapsule.name);
        }
        else
        {
            Debug.LogWarning("Couldn't find '---------- ENVIRONMENT ----------' in scene CJOLI.");
        }

        Islands[0] = FindGameObjectInScene("CJOLI", "SM_Isle1");
        if (EnvironmentCapsule != null)
        {
            Debug.Log("Found Island object: " + Islands[0].name);
        }
        else
        {
            Debug.LogWarning("Couldn't find 'SM_Isle1' in scene CJOLI.");
        }

        Islands[1] = FindGameObjectInScene("CJOLI", "SM_Isle2");
        if (EnvironmentCapsule != null)
        {
            Debug.Log("Found Island object: " + Islands[1].name);
        }
        else
        {
            Debug.LogWarning("Couldn't find 'SM_Isle2' in scene CJOLI.");
        }

        Islands[2] = FindGameObjectInScene("CJOLI", "SM_Isle3");
        if (EnvironmentCapsule != null)
        {
            Debug.Log("Found Island object: " + Islands[2].name);
        }
        else
        {
            Debug.LogWarning("Couldn't find 'SM_Isle3' in scene CJOLI.");
        }
    }

    IEnumerator FindFlamSpawnpoints()
    {
        yield return new WaitForSeconds(2);

        List<GameObject> list = FindAllObjectWithNameInScene("E_Flam_Spawnpoints");


    }

    List<GameObject> FindAllObjectWithNameInScene(string sceneName)
    {
        return null;
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
}
