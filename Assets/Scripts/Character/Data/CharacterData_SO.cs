using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Player", menuName = "Data/Player")]
public class CharacterData_SO : ScriptableObject
{
    [Header("属性点")]
    public int 等级;
    public int 体质;
    public int 力量;
    public int 敏捷;
    public int 反应;
    public int 剩余属性点;
    public int 生命值
    {
        get { return 50 + 体质 * 3; }
    }
    public int 攻击力
    {
        get { return (int)(8 + 体质 * 0.1 + 力量 * 0.8 + 敏捷 * 0.1); }
    }
    public double 攻击速度
    {
        get { return 0.6 - 敏捷 * 0.03 - 反应 * 0.01; }
    }
    public int 挥刀次数
    {
       get{return (int)(2+体质*0.1+力量*0.1+敏捷*0.3+反应*0.3);}
    }
    public double 剩余时间
    {
        get { return 2 + 反应 * 0.3; }
    }
}
