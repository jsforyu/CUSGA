using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Quest",menuName ="Quest/QuestData")]
public class QuestData_SO : ScriptableObject
{
    [Header("任务名字")]
    public string questName;
    [Header("任务描述")]
    public string description;

    public bool isStarted;
    public bool isComplete;
    public bool isFinished;
    public List<QuestRequire> questRequires = new List<QuestRequire>();
}
[System.Serializable]
public class QuestRequire
{
    public string name;
    public int requireAmount;
    public int currentAmount;
}

