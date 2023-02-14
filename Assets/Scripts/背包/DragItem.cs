using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviourPunCallbacks, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder; //当前格子
    SlotHolder targetHolder;  //目标格子
    Transform drag;
    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
        drag = GameObject.Find("Drag Canvas").transform;
    }
  public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.currentDrag = new InventoryManager.DragData();
        InventoryManager.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.currentDrag.originalParent = (RectTransform)transform.parent;
        //记录原始数据 回到原来的格子里
        transform.SetParent(drag, true);
    }
   public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置
        transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品交换数据
        //是否指向UI物品
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //判断鼠标位置是否在三个格子内
            if (GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().CheckInActionUI(eventData.position) ||
                GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().CheckInEquipmentUI(eventData.position)
                || GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().CheckInInventoryUI(eventData.position))
            {
                //判断鼠标进入的格子里是否有物品
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                   else
                   targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
        switch (targetHolder.slotType)
        {
            case SlotType.BAG:
                            SwapItem();
                break;
            case SlotType.WEAPON:
                        if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType == ItemType.Weapon)
                            SwapItem();
                        break;
            case SlotType.ARMOR:
                        if (currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType == ItemType.Armor)
                            SwapItem();
                        break;
            case SlotType.ACTION:
                    if(currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType==ItemType.Useable)
                        SwapItem();
                        break;
            //case SlotType.TRASH:
            //            Vector3 position = GameObject.Find("Player").GetComponent<Transform>().position;
            //            position += new Vector3(0, 1, 5);
            //            PhotonNetwork.Instantiate("Sword_world", position, Quaternion.identity, 0);
            //            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = null;
            //            break;
                }
                currentHolder.UpdateItem();
        targetHolder.UpdateItem();
    }
       }
        //else
        //{   
        //    Vector3 position=GameObject.Find("Player").GetComponent<Transform>().position;
        //    position += new Vector3(0, 0, 5);
        //    GameObject a =Instantiate(currentHolder.itemUI.GetItem().weaponPrefab);
        //    a.transform.position = position;
        //    currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = null;
        //}

        //将物品放回原来的父节点下面
        //Vector3 position = GameObject.Find("Player").GetComponent<Transform>().position;
        //position += new Vector3(0, 0, 5);
        //PhotonNetwork.Instantiate("Sword_world", position, Quaternion.identity, 0);
        //currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = null;
        transform.SetParent(InventoryManager.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }
    public void SwapItem()
{
    var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
    var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
    //判断是不是同一个物品
        bool isSameItem = tempItem.ItemData == targetItem.ItemData; 
    if (isSameItem && targetItem.ItemData.stackable)
    {
        targetItem.amount += tempItem.amount;
        tempItem.ItemData = null;
        tempItem.amount = 0;
    }
    else
    {
        currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
        targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
    }
}
}
