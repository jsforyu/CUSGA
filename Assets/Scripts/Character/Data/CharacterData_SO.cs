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
    public float �������ֵ
    {
        get { return 50 + ���� * 3; }
    }
    [HideInInspector]
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
        return (float)(0.5 * (enemy.��Ӧ + enemy.���� * 0.6 - player.���� * 0.4 - player.��Ӧ * 0.2));
    }
    public float ��������
    {
        get { return 50 + �ȼ� * 5; }
    }
    [HideInInspector]
    public float ��ǰ������;
}
