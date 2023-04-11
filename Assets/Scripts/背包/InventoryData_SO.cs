using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Inventroy",menuName="Inventory/Inventory")]
public class InventoryData_SO : ScriptableObject
{
    //充当背包功能 
    public List<InventoryItem> items = new List<InventoryItem>();
    public ItemData_SO currentJianJi;
    public void AddItem(ItemData_SO newItemData,int amount)
    {
        bool found = false;
        for (int i = 0; i < items.Count; i++)
        {

            if (items[i].ItemData == newItemData && !found)//找到了在背包里
            {
                items[i].amount += amount;
                newItemData.itemAmount += amount;
                found = true;
                break;
            }
        }
        for (int i = 0; i < items.Count; i++)//背包有空位可以放
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
        if (!found)//背包里没空位
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
        //背包满了
        Debug.Log("加了");
    } //添加物品的方法
}
[System.Serializable]  //序列化 在列表中可以看见
public class InventoryItem
{
    public ItemData_SO ItemData;
    public int amount;
}
