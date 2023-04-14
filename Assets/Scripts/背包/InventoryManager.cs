using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
//�����Ĺ��� �б��汳���Ͷ�ȡ��������  ���Դ���һ������������ű�  �����·ű�����ui
{ public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    public GameObject player;
    public static InventoryManager instance;
    public int baghave = 3;
    public ItemData_SO FirstJianJi;
    //��������
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;//��ͬ����
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentData;
    [Header("ContainerS")]
    public ContainerUI inventoryUI;//��ͬ������UI
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

    public AudioSource btnAudioSource;


    private void Start()
    {
        // DontDestroyOnLoad(this);
        //Cursor.visible = false;
        //LoadData();    //��������
        Debug.Log("������С"+inventoryData.items.Count);
        inventoryUI.RefreshUI();
        //inventoryData.currentJianJi = inventoryData.items[0].ItemData;
        //actionUI.RefreshUI();
        //equipmentUI.RefreshUI();

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void Update()
    {
        //SavaData();  //��������
        if (Input.GetKeyDown(KeyCode.B))   //��or�رձ���
        {
            isOpen = !isOpen;
            //Cursor.visible = isOpen;
            BagCanvas.SetActive(isOpen);
            //CharactersCanvas.SetActive(isOpen);
            //if (isOpen == false) { tooltip.SetupTooltip(null); }
            //tooltip.gameObject.SetActive(isOpen);
            
        }
        //����������Ϣ
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
    #region �����ק��Ʒ�Ƿ���ÿһ��slot��Χ��
    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Count; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))//�ж�position�Ƿ���t�ķ�Χ��
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

    #region ���������Ʒ
    public void CheckQuestItemInBag(string questItemName)
    {
        foreach(var item in inventoryData.items)
        {
            if(item.ItemData!=null)
            {
                if(item.ItemData.itemName==questItemName)
                {
                    QuestManager.Instance.UpdateQuestProgress(item.ItemData.itemName, item.amount);
                }
            }
        }
    }
    #endregion
    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.ItemData == questItem);
    }
    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.ItemData == questItem);
    }
    public void BagOpenButton()
    {
        btnAudioSource.Play();
        isOpen = !isOpen;
        //Cursor.visible = isOpen;
        BagCanvas.SetActive(isOpen);
        //CharactersCanvas.SetActive(isOpen);
        //if (isOpen == false) { tooltip.SetupTooltip(null); }
        //tooltip.gameObject.SetActive(isOpen);
    }

}

