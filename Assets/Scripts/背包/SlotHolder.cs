using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//���ñ��� ���� ���Ƹ���  Ҳ��������������Ь�� ͷ��
public enum SlotType { BAG,WEAPON,ARMOR,ACTION,TRASH}
public class SlotHolder : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    //����һ������ ����Inventory Canvas ����Inventory Manager�ű� �䵱��������
    //��Inventory Canvas�´���һ��ͼƬ���� Inventory Bag �䵱��������
    //��Inventory Bag�´���һ��Panel�������и��� ����Inventory Contaniner ����Container UI ��Grid Layout Group���
    //�������ӵĴ�����
    //����һ��Image ����SlotHolder ���ص�ǰ�ű� ImageΪ�������ӵ�ͼƬ
    //�����������´���һ�������� ���� ItemSlot ����Item UI �� Drag Item �ű�
    //ItemSlot�������´���ͼƬ ��ʾ��ǰ��Ʒ��ͼƬ ������ͼƬ�´���text ��ʾ��ǰ����
    //��SlotHolder��ΪԤ���� SlotHolder��Ϊ����
    public InventoryData_SO inventoryData;  //�������ݿ�
    public InventoryData_SO equipmentData;
    public InventoryData_SO actionData;
    public SlotType slotType;
    public ItemUI itemUI;
    public GameObject tooptip;
    bool isclick;
    private void Update()
    {
        if (isclick == true && Input.GetMouseButtonDown(0))
        {
                if (transform.GetChild(0).gameObject.activeSelf == true)
                {
                    ItemData_SO temp;
                    temp = itemUI.Bag.items[itemUI.Index].ItemData;
                    InventoryManager.instance.inventoryData.currentJianJi = temp;
                    PlayerInUI.Instance.slot.transform.GetChild(0).GetChild(0).GetComponent<ItemUI>().SetupItemUI(temp, temp.itemAmount);
                    PlayerInUI.Instance.slot.transform.GetChild(1).GetComponent<Text>().text = "��װ������"+temp.itemName;
                    PlayerInUI.Instance.slot.transform.GetChild(2).GetComponent<Text>().text = InventoryManager.instance.inventoryData.currentJianJi.description;
                isclick = false;
                }
        }
        RefreshitemUI();
    }
    private void Awake()
    {
        tooptip = GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject;
        //ResfeshitemUI();
    }
    private void Start()
    {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
    }
    public void OnPointerEnter(PointerEventData eventData) //������Ƶ���ǰ����ʱ ���µ���˵�������ı�
    {
        if(itemUI.GetItem())
        {
            GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.SetupTooltip(itemUI.GetItem());
            GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(true);
            isclick = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData) //������Ƴ�����յ���˵�����ı�
    {
        isclick = false;
        GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
    }
      public void OnDisable() //���ر�ʱ���� ��ֱֹ�ӹرձ�������˵����û����
    {
        //GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
        tooptip.SetActive(false);
    }
    public void RefreshitemUI()
    {
        if (itemUI.GetItem())
        {
            Debug.Log(itemUI.GetItem().itemAmount);
            itemUI.SetupItemUI(itemUI.GetItem(), itemUI.GetItem().itemAmount);
        }
        else itemUI.gameObject.SetActive(false);
    }
}
