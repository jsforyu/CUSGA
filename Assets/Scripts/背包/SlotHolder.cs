using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
//设置背包 武器 盾牌格子  也可设其他的例如鞋子 头盔
public enum SlotType { BAG,WEAPON,ARMOR,ACTION,TRASH}
public class SlotHolder : MonoBehaviourPunCallbacks, IPointerEnterHandler,IPointerExitHandler
{
    public InventoryData_SO inventoryData;  //背包数据库
    public InventoryData_SO equipmentData;
    public InventoryData_SO actionData;
    public SlotType slotType;
    public ItemUI itemUI;
    public GameObject tooptip;
    private void Awake()
    {
        tooptip = GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject;
    }
    public void UseItem()  //使用物品
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
                //装备武器 切换武器
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
      public void OnDisable() //被关闭时启用
    {
        //GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
        tooptip.SetActive(false);
    }
}
