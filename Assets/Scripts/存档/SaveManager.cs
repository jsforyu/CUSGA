using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public CharacterData_SO currentStats;
    public InventoryData_SO bagSO;
    public EventSO eventSO;
    private void Start()
    {
        LoadPlayerData();
        DontDestroyOnLoad(gameObject);
    }
    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))  //�������
        //{
        //    PlayerPrefs.DeleteAll();
        //    LoadPlayerData();
        //}
        SavePlayerData();
    }
    public void SavePlayerData()
    {
        Save(currentStats, currentStats.name);
        Save(bagSO, bagSO.name);
        Save(eventSO, eventSO.name);
    }
    public void LoadPlayerData()
    {
        Load(currentStats, currentStats.name);
        Load(bagSO, bagSO.name);
        Load(eventSO, eventSO.name);
    }
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}