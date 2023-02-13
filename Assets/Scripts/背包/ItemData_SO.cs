using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType{Useable,Weapon,Armor}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;  //图标
    public int itemAmount;
    [TextArea]
    public string description = "";
    public bool stackable; //判断是否可以堆叠
    [Header("Useable Item")]
    public UseableData_SO itemData;
    public GameObject useablePrefab;
    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
    public AnimatorOverrideController weaponAnimator;
}
