using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MaterialChanger
{
    public Material Material;
    public Texture2D TextureBoo;
    public Texture2D TextureDarkBoo;
}

[System.Serializable]
public struct WaterChanger
{
    public GameObject Water;
    public Material MaterialBoo;
    public Material MaterialDarkBoo;
}
public class ChangeMaterialManager : MonoBehaviour, IChangeable
{

    [SerializeField] List<MaterialChanger> _materials;
    [SerializeField] WaterChanger _waterChanger;
    

    public void Change()
    {
        foreach(MaterialChanger changer in _materials)
        {
            changer.Material.SetTexture("_BaseMap", changer.TextureDarkBoo);
        }
        _waterChanger.Water.GetComponent<MeshRenderer>().material = _waterChanger.MaterialDarkBoo;
    }

    public void ResetChange()
    {
        foreach (MaterialChanger changer in _materials)
        {
            changer.Material.SetTexture("_BaseMap", changer.TextureBoo);
        }
        _waterChanger.Water.GetComponent<MeshRenderer>().material = _waterChanger.MaterialBoo;
    }


    // Start is called before the first frame update
    void Start()
    {

        AngrySystem.Instance.OnChangeElements += Change;
        AngrySystem.Instance.OnResetElements += ResetChange;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        foreach (MaterialChanger changer in _materials)
        {
            changer.Material.SetTexture("_BaseMap", changer.TextureBoo);
        }
        if(_waterChanger.Water != null)
        {
            _waterChanger.Water.GetComponent<MeshRenderer>().material = _waterChanger.MaterialBoo;
        }
        if(AngrySystem.Instance != null)
        {
            AngrySystem.Instance.OnChangeElements += Change;
            AngrySystem.Instance.OnResetElements += ResetChange;
        }
        
    }
}
