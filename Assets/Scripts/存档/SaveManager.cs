using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public CharacterData_SO currentStats;
    public InventoryData_SO bagSO;
    public EventSO eventSO;
    public PlayeInMapData mapdata;
    private void Start()
    {
        // PlayerPrefs.DeleteAll();
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
    }

    public void InitPlayerData()
    {
        // 玩家属性
        currentStats.等级 = 1;
        currentStats.体质 = 1;
        currentStats.力量 = 1;
        currentStats.敏捷 = 1;
        currentStats.反应 = 1;
        currentStats.剩余属性点 = 0;
        currentStats.经验值 = 0;
        // 背包内容
        bagSO.items[1].ItemData = null;
        bagSO.items[1].amount = 0;
        bagSO.items[2].ItemData = null;
        bagSO.items[2].amount = 0;
        bagSO.currentJianJi = bagSO.items[0].ItemData;
        // 事件列表
        for (int i = 0; i < eventSO.events.Length; i++)
        {
            eventSO.events[i] = false;
        }
        eventSO.gameVolume = 1;
        // 地图数据
        mapdata.mapindex = 10;
    }
    public void SavePlayerData()
    {
        Save(currentStats, currentStats.name);
        Save(bagSO, bagSO.name);
        Save(eventSO, eventSO.name);
        Save(mapdata, mapdata.name);
    }
    public void LoadPlayerData()
    {
        Load(currentStats, currentStats.name);
        Load(bagSO, bagSO.name);
        Load(eventSO, eventSO.name);
        Load(mapdata, mapdata.name);
    }
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}