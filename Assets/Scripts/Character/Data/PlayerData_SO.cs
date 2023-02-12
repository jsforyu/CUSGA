using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "Data/Player")]
public class PlayerData_SO : ScriptableObject
{
    [Header("等级")]
    public int Level;
    [Header("体质")]
    public int Physique;       //决定生命值
    [Header("生命值")]
    public int Health;
    [Header("力量")]
    public int Strength;     //决定攻击力
    [Header("攻击力")]
    public int Attack_Strength;
    [Header("敏捷")]
    public int Agility;      //决定攻击速度
    [Header("攻击速度")]
    public int Attack_Agility;
    [Header("属性点")]
    public int Attribute_points;
}
