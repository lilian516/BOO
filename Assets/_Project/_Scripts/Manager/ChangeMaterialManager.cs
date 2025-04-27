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
public class ChangeMaterialManager : MonoBehaviour, IChangeable
{

    [SerializeField] List<MaterialChanger> _materials;

    public void Change()
    {
        foreach(MaterialChanger changer in _materials)
        {
            changer.Material.SetTexture("_BaseMap", changer.TextureDarkBoo);
        }
    }

    public void ResetChange()
    {
        foreach (MaterialChanger changer in _materials)
        {
            changer.Material.SetTexture("_BaseMap", changer.TextureBoo);
        }
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
}
