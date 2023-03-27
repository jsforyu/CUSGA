using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
//背包的管理 有保存背包和读取背包功能  可以创建一个画布放这个脚本  画布下放背包的ui
{ public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    public GameObject player;
    public static InventoryManager instance;
    //保存数据
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;//不同背包
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;
    [Header("ContainerS")]
    public ContainerUI inventoryUI;//不同背包的UI
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;
    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    static public DragData currentDrag;
    private bool isOpen = false;
    public GameObject BagCanvas;
    public GameObject CharactersCanvas;
    [Header("Stats Text")]
    public Text healthText;
    public Text attackText;
    [Header("Tooltip")]
    public ItemTooltip tooltip;

    private void Start()
    {
        Cursor.visible = false;
        //LoadData();    //加载数据
        inventoryUI.RefreshUI();
        //actionUI.RefreshUI();
        //equipmentUI.RefreshUI();

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void Update()
    {
        //SavaData();  //保存数据
        if (Input.GetKeyDown(KeyCode.B))   //打开or关闭背包
        {
            isOpen = !isOpen;
            Cursor.visible = isOpen;
            BagCanvas.SetActive(isOpen);
            //CharactersCanvas.SetActive(isOpen);
            tooltip.gameObject.SetActive(isOpen);
            
        }
        //更新人物信息
        //UpdateStatsText(player.GetComponent<CharacterStats>().characterData.maxHealth, player.GetComponent<CharacterStats>().attackData.minDamge, player.
        //    GetComponent<CharacterStats>().attackData.maxDamge);
    }
    public void SavaData()
    {
        Save(inventoryData, inventoryData.name);
        //Save(actionData, actionData.name);
        //Save(equipmentData, equipmentData.name);
    }
    public void LoadData()
    {
        Load(inventoryData, inventoryData.name);
        //Load(actionData, actionData.name);
        //Load(equipmentData, equipmentData.name);
    }
    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
    public void UpdateStatsText(int health, int min, int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + " - " + max;
    }
    #region 检查拖拽物品是否在每一个slot范围内
    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Count; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))//判断position是否在t的范围内
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Count; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInEquipmentUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Count; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}

