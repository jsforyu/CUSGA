using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviourPunCallbacks, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder; //��ǰ����
    SlotHolder targetHolder;  //Ŀ�����
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
        //��¼ԭʼ���� �ص�ԭ���ĸ�����
        transform.SetParent(drag, true);
    }
   public void OnDrag(PointerEventData eventData)
    {
        //�������λ��
        transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //������Ʒ��������
        //�Ƿ�ָ��UI��Ʒ
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //�ж����λ���Ƿ�������������
            if (GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().CheckInActionUI(eventData.position) ||
                GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().CheckInEquipmentUI(eventData.position)
                || GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().CheckInInventoryUI(eventData.position))
            {
                //�ж�������ĸ������Ƿ�����Ʒ
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

        //����Ʒ�Ż�ԭ���ĸ��ڵ�����
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
    //�ж��ǲ���ͬһ����Ʒ
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
