using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "Data/Player")]
public class PlayerData_SO : ScriptableObject
{
    [Header("�ȼ�")]
    public int Level;
    [Header("����")]
    public int Physique;       //��������ֵ
    [Header("����ֵ")]
    public int Health;
    [Header("����")]
    public int Strength;     //����������
    [Header("������")]
    public int Attack_Strength;
    [Header("����")]
    public int Agility;      //���������ٶ�
    [Header("�����ٶ�")]
    public int Attack_Agility;
    [Header("���Ե�")]
    public int Attribute_points;
}
