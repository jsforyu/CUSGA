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
    public int 最大生命值
    {
        get { return 50 + 体质 * 3; }
    }
    public int 当前生命值;
    public int 攻击力
    {
        get { return (int)(8 + 体质 * 0.1 + 力量 * 0.8 + 敏捷 * 0.1); }
    }
    public float 攻击速度
    {
        get {
            if (敏捷 <= 10)
            { return (float)(0.6 - 敏捷 * 0.02 - 反应 * 0.01); }
            return (float)(0.6 - 敏捷 * 0.005 - 反应 * 0.01);
        }
    }
    public int 挥刀次数
    {
        get { return (int)(2 + 体质 * 0.1 + 力量 * 0.1 + 敏捷 * 0.3 + 反应 * 0.3); }
    }
    public float 剩余时间
    {
        get { return (float)(2 + 反应 * 0.3); }
    }
    public float Block(CharacterData_SO player,CharacterData_SO enemy)
    {
        return (float)(0.5 * (enemy.反应 + enemy.敏捷 * 0.6 - player.敏捷 * 0.4 - player.反应 * 0.2));
    }
    public float 当前架势条;
    public float 最大架势条;
}
