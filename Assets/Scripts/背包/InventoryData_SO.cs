using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Inventroy",menuName="Inventory/Inventory")]
public class InventoryData_SO : ScriptableObject
{
    //�䵱�������� 
    public List<InventoryItem> items = new List<InventoryItem>();
    public ItemData_SO currentJianJi;
    public void AddItem(ItemData_SO newItemData,int amount)
    {
        bool found = false;
        for (int i = 0; i < items.Count; i++)
        {

            if (items[i].ItemData == newItemData && !found)//�ҵ����ڱ�����
            {
                items[i].amount += amount;
                newItemData.itemAmount += amount;
                found = true;
                break;
            }
        }
        for (int i = 0; i < items.Count; i++)//�����п�λ���Է�
        {

            if (items[i].ItemData == null && !found)
            {
                items[i].ItemData = newItemData;
                items[i].amount = amount;
                newItemData.itemAmount = amount;
                found = true;
                break;
            }
        }
        if (!found)//������û��λ
        {
            InventoryItem tempitem = new InventoryItem();
            tempitem.ItemData = newItemData;
            tempitem.amount = amount;
            items.Add(tempitem);
        }
        //if (newItemData.stackable)
        //{
        //    foreach(var item in items)
        //    {
        //        if(item.ItemData == newItemData)
        //        {
        //            item.amount += amount;
        //            found = true;
        //            break;
        //        }
        //    }
        //}
        //��������
        Debug.Log("����");
    } //�����Ʒ�ķ���
}
[System.Serializable]  //���л� ���б��п��Կ���
public class InventoryItem
{
    public ItemData_SO ItemData;
    public int amount;
}
