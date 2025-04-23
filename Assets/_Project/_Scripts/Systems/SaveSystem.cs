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

    private bool _isAllowedToSave;
    public bool IsAllowedToSave { get => _isAllowedToSave; set => _isAllowedToSave = value; }

    private void Start()
    {

        _filePath = Application.persistentDataPath + "save.txt";
        if (File.Exists(_filePath))
        {
            LoadAllData();
        }
        else
        {
            Data = GetDefaultData();
            _isAllowedToSave = true;
        }
    }

    private void Update()
    {
        if (_isAllowedToSave) 
        { 
            SaveAllData();
            _isAllowedToSave = false;
        }
    }

    private Dictionary<string, object> GetDefaultData()
    {
        return new Dictionary<string, object>
        {
            {"test", 1},
            {"AAAAAH", false},
            {"MusicVolume", 1},
            {"MagnitudeIndex", 0}
        };
    }

    public void SaveElement<T>(string key, T value)
    {
        Data[key] = value;
        _isAllowedToSave = true;
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
        if (!_isAllowedToSave)
        {
            _isAllowedToSave = true;
        }

        string JSON = JsonConvert.SerializeObject(Data);

        if (!File.Exists(_filePath))
        {
            using (File.CreateText(_filePath)) {  }
        }

        File.WriteAllText(_filePath, JSON);

    }

    public void LoadAllData()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }

        string JSONString = File.ReadAllText(_filePath);

        if(string.IsNullOrEmpty(JSONString))
        {
            Data = GetDefaultData();
        }

        Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JSONString);
    }

    public void ResetElement<T>(string key)
    {
        if (Data.ContainsKey(key) && GetDefaultData().ContainsKey(key))
        {
            Data[key] = GetDefaultData()[key];
            _isAllowedToSave = true;
        }
    }
   public void ResetAllData()
    {
        Data = GetDefaultData();
        _isAllowedToSave = true;
    }
}
