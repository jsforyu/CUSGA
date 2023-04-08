using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemUI : MonoBehaviour
{
    //判断背包中对应当前格子是否有物品 有的话将Image激活  无则隐藏
    public Image icon = null;
    public Text amount = null;
    public ItemData_SO currentItemData;
    public InventoryData_SO Bag;
    public int Index;

    public void SetupItemUI(ItemData_SO item,int itemAmount)  
    {
        if (item == null)
        {
            Bag.items[Index].ItemData = null;
            icon.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            return;
        }
        if(itemAmount==0)
        {
            Bag.items[Index].ItemData = null;
            icon.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            return;
        }
        if(itemAmount<0)
        {
            item = null;
        }
        if(item!=null)
        {
            currentItemData= item;
            this.gameObject.SetActive(true);
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
