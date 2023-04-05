using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    //���ǿ����϶����ߵ��������� ���紩��װ�� ����ʹ�õĵ����ϵ�������
    ItemUI currentItemUI;
    SlotHolder currentHolder; //��ǰ����
    SlotHolder targetHolder;  //Ŀ�����
    Transform drag;
    Transform imageui;
    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
        //drag = GameObject.Find("Drag Canvas").transform;
    }
  public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.currentDrag = new InventoryManager.DragData();
        InventoryManager.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.currentDrag.originalParent = (RectTransform)transform.parent;
        imageui = transform.GetChild(0);
        imageui.GetComponent<Image>().raycastTarget = false;//���ټ��
        transform.SetParent(transform.parent.parent);

        //��¼ԭʼ���� �ص�ԭ���ĸ�����
        //transform.SetParent(drag, true);
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
                Debug.Log("��ʼ�ж�");
                Debug.Log("���" + eventData.pointerEnter.gameObject.GetComponent<SlotHolder>());
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    //û������
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                    Debug.Log("hh"+targetHolder.itemUI.Index);
                }
                else
                { 
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                    Debug.Log("yy"+targetHolder.itemUI.Index);
                
                }
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
                }
        }
            }
        transform.SetParent(InventoryManager.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
        imageui = transform.GetChild(0);
        imageui.GetComponent<Image>().raycastTarget =true;//�ټ��
    }

    public void SwapItem()   //��ק����һ������ʱ�ж�
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
        Debug.Log(targetItem.ItemData);
        //�ж��ǲ���ͬһ����Ʒ
        if(targetHolder.itemUI.Index== currentHolder.itemUI.Index)
        {
            //��ͬһ������
            transform.SetParent(InventoryManager.currentDrag.originalParent);
            RectTransform t = transform as RectTransform;
            t.offsetMax = -Vector2.one * 5;
            t.offsetMin = Vector2.one * 5;
            return;
        }
            bool isSameItem = tempItem.ItemData == targetItem.ItemData; 
        if (isSameItem && targetItem.ItemData.stackable)  //��ͬһ������������1
        {
            targetItem.ItemData.itemAmount += tempItem.ItemData.itemAmount;//�������е�
            targetItem.amount = targetItem.ItemData.itemAmount;
            tempItem.ItemData.itemAmount = 0; //�������е�
            tempItem.ItemData = null;
            tempItem.amount = 0;
            
        }
        else  //����ݻ�
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
