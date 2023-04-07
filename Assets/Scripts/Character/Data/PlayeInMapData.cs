using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New PlayerInMap", menuName = "PlayerInMap/PlayerData")]
public class PlayeInMapData : ScriptableObject
{
    //记录角色在地图的数据
    public int mapindex;//所在点的下标

}
