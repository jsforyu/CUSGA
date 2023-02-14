using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType{Useable,Weapon,Armor}
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    //����һ����Ʒģ��
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;  //ͼ��
    public int itemAmount;
    [TextArea]
    public string description = "";
    public bool stackable; //�ж��Ƿ���Զѵ�
}
