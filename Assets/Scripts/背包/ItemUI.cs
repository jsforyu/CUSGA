using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemUI : MonoBehaviour
{
    //判断背包中对应当前格子是否有物品 有的话将Image激活  无则隐藏
    public Image icon = null;
    public Text amount = null;
    public InventoryData_SO Bag { get; set; }
    public int Index { get; set; } = -1;
    public void SetupItemUI(ItemData_SO item,int itemAmount)  
    {
        if(itemAmount==0)
        {
            Bag.items[Index].ItemData = null;
            icon.gameObject.SetActive(false);
            return;
        }
        if(item!=null)
        {
            icon.sprite = item.itemIcon;
            amount.text=itemAmount.ToString(); 
            icon.gameObject.SetActive(true);
        }
            else
            icon.gameObject.SetActive(false);
    }
    public ItemData_SO GetItem()
    {
        return Bag.items[Index].ItemData;
    }
}
