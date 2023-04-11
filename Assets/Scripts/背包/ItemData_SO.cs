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

    // 剑技效果
    [Tooltip("攻击倍率")]
    public float[] attackMultiplier;
    [Tooltip("字符移动速度倍率")]
    public float[] abSpeedMultiplier;
    [Tooltip("生命恢复相对于攻击力的倍率")]
    public float[] healMultiplier;
    [Tooltip("架势条恢复量")]
    public float angerRecovery;
}
