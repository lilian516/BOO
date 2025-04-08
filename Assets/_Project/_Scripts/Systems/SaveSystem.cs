using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class SaveSystem : Singleton<SaveSystem>
{
    [HideInInspector] public Dictionary<string, object> Data;

    private string _filePath;

    private void Start()
    {
        _filePath = Application.dataPath + "/_Project/Resources/Saves/" + "save.txt";

        // Put default keys and values in the declaration below
        Data = new Dictionary<string, object>{
            {"test", 1},
            {"AAAAAH", false},
            {"MusicVolume", 1}
        };
    }

    private void Update()
    {
        SaveAllData();
        LoadAllData();
    }

    public void SaveElement<T>(string key, T value)
    {
        Data[key] = value;
    }

    public T LoadElement<T>(string key)
    {
        if (Data.ContainsKey(key))
        {
            Data.TryGetValue(key, out object value);

            if (value is T typedValue)
            {
                return typedValue;
            }
        }

        return default(T);
    }

    public void SaveAllData()
    {
        string JSON = JsonConvert.SerializeObject(Data);

        if (!File.Exists(_filePath))
        {
            File.CreateText(_filePath);
        }

        File.WriteAllText(_filePath, JSON);
    }

    public void LoadAllData()
    {
        string JSONString = File.ReadAllText(_filePath);

        if (!File.Exists(_filePath))
        {
            return;
        }
        Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONString);
    }
}
