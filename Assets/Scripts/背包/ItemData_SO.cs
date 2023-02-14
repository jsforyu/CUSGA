using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType{Useable,Weapon,Armor}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    //创建一个物品模板
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;  //图标
    public int itemAmount;
    [TextArea]
    public string description = "";
    public bool stackable; //判断是否可以堆叠
}
