using System;
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
    public float 攻击符速度;
    [Tooltip("在玩家数据中代表当前经验值，在敌人数据中代表胜利获得经验值")]
    public int 经验值;

    public float 最大生命值
    {
        get { return 150 + 体质 * 3; }
    }
    [NonSerialized]
    public float 当前生命值;
    public float 攻击力
    {
        get { return (int)(8 + 体质 * 0.1 + 力量 * 0.8 + 敏捷 * 0.1); }
    }
    public int 挥刀次数
    {
        get { return (int)(2 + 体质 * 0.1 + 力量 * 0.1 + 敏捷 * 0.3 + 反应 * 0.3); }
    }
    public float BlockProb(CharacterData_SO player,CharacterData_SO enemy)  // 敌人格挡概率
    {
        return (float)(0.5 +enemy.反应*0.1 + enemy.敏捷 * 0.06 - player.敏捷 * 0.04 - player.反应 * 0.02);
        
    }
    public float 最大架势条
    {
        get { return 150 + 等级 * 5; }
    }
    [NonSerialized]
    public float 当前架势条;

    public int 升级所需经验值
    {
        get { return (int)(100 * Math.Pow(1.4, 等级 - 1)); }
    }
        
    public void 获得经验值(int addExp)
    {
        经验值 += addExp;
        while (true)
        {
            if (经验值 >= 升级所需经验值)
            {
                经验值 -= 升级所需经验值;
                等级++;
                剩余属性点 += 2;
            }
            else
            {
                break;
            }
        }
    }
}
