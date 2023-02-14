using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
//���ñ��� ���� ���Ƹ���  Ҳ��������������Ь�� ͷ��
public enum SlotType { BAG,WEAPON,ARMOR,ACTION,TRASH}
public class SlotHolder : MonoBehaviourPunCallbacks, IPointerEnterHandler,IPointerExitHandler
{
    public InventoryData_SO inventoryData;  //�������ݿ�
    public InventoryData_SO equipmentData;
    public InventoryData_SO actionData;
    public SlotType slotType;
    public ItemUI itemUI;
    public GameObject tooptip;
    private void Awake()
    {
        tooptip = GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject;
    }
    public void UseItem()  //ʹ����Ʒ
    {
        if(itemUI.GetItem()!=null)
        if(itemUI.GetItem().itemType==ItemType.Useable&&itemUI.Bag.items[itemUI.Index].amount>0)
        {
            GameObject.Find("Player").GetComponent<CharacterStats>().ApplyHealth(itemUI.GetItem().itemData.healthPoint);
                itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        UpdateItem();
    }
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = inventoryData;
                break;
                case SlotType.WEAPON:
                itemUI.Bag = equipmentData;
                //װ������ �л�����
                if (itemUI.GetItem() != null)
                {
                    if (!photonView.IsMine && PhotonNetwork.IsConnected)
                        return;
                    //GameObject.Find("Player").GetComponent<CharacterStats>().ChangeWeapon(itemUI.Bag.items[itemUI.Index].ItemData);
                    GameObject.Find("Player").GetComponent<CharacterStats>().ChangeWeapon(itemUI.GetItem());
                }
                else
                {
                    if (!photonView.IsMine && PhotonNetwork.IsConnected)
                        return;
                    GameObject.Find("Player").GetComponent<CharacterStats>().UnEquipWeapon();
                    GameObject.Find("Player").GetComponent<CharacterStats>().can = true;
                    //photonView.RPC("ReChangeWeapon", RpcTarget.AllBuffered);
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = equipmentData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = actionData;
                break;
        }
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.ItemData,item.amount);
    }

   public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI.GetItem())
        {
            GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.SetupTooltip(itemUI.GetItem());
            GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
    }
      public void OnDisable() //���ر�ʱ����
    {
        //GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
        tooptip.SetActive(false);
    }
}
