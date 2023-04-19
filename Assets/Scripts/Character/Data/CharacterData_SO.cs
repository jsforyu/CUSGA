using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "Data/Player")]
public class CharacterData_SO : ScriptableObject
{
    [Header("���Ե�")]
    public int �ȼ�;
    public int ����;
    public int ����;
    public int ����;
    public int ��Ӧ;
    public int ʣ�����Ե�;
    public float �������ٶ�;
    [Tooltip("����������д���ǰ����ֵ���ڵ��������д���ʤ����þ���ֵ")]
    public int ����ֵ;

    public float �������ֵ
    {
        get { return 150 + ���� * 3; }
    }
    [NonSerialized]
    public float ��ǰ����ֵ;
    public float ������
    {
        get { return (int)(8 + ���� * 0.1 + ���� * 0.8 + ���� * 0.1); }
    }
    public int �ӵ�����
    {
        get { return (int)(2 + ���� * 0.1 + ���� * 0.1 + ���� * 0.3 + ��Ӧ * 0.3); }
    }
    public float BlockProb(CharacterData_SO player,CharacterData_SO enemy)  // ���˸񵲸���
    {
        return (float)(0.5 +enemy.��Ӧ*0.1 + enemy.���� * 0.06 - player.���� * 0.04 - player.��Ӧ * 0.02);
        
    }
    public float ��������
    {
        get { return 150 + �ȼ� * 5; }
    }
    [NonSerialized]
    public float ��ǰ������;

    public int �������辭��ֵ
    {
        get { return (int)(100 * Math.Pow(1.4, �ȼ� - 1)); }
    }
        
    public void ��þ���ֵ(int addExp)
    {
        ����ֵ += addExp;
        while (true)
        {
            if (����ֵ >= �������辭��ֵ)
            {
                ����ֵ -= �������辭��ֵ;
                �ȼ�++;
                ʣ�����Ե� += 2;
            }
            else
            {
                break;
            }
        }
    }
}
