using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public CharacterData_SO currentStats;
    public InventoryManager inventoryManager;
    private void Start()
    {
        LoadPlayerData();
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
        //SavePlayerData();
    }
    public void SavePlayerData()
    {
        Save(currentStats, currentStats.name);
        Save(inventoryManager.inventoryData, inventoryManager.inventoryData.name);
    }
    public void LoadPlayerData()
    {
        Load(currentStats, currentStats.name);
        Load(inventoryManager.inventoryData, inventoryManager.inventoryData.name);
    }
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}