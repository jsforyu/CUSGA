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
    public int ����ֵ
    {
        get { return 50 + ���� * 3; }
    }
    public int ������
    {
        get { return (int)(8 + ���� * 0.1 + ���� * 0.8 + ���� * 0.1); }
    }
    public float �����ٶ�
    {
        get { return (float)(0.6 - ���� * 0.03 - ��Ӧ * 0.01); }
    }
    public int �ӵ�����
    {
       get{return (int)(2+����*0.1+����*0.1+����*0.3+��Ӧ*0.3);}
    }
    public float ʣ��ʱ��
    {
        get { return (float)(2 + ��Ӧ * 0.3); }
    }
}
