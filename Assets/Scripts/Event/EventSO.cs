using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class EventSO : ScriptableObject
{
    public bool[] events;
    public int currentevent;
    public ItemData_SO accessibleSkill;
    // 游戏音量我也存储在这了
    public float gameVolume;
}
