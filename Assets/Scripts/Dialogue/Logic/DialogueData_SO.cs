using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Dialogue",menuName ="Dialogue/DialogueData")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();
    //void OnValidate()//仅在编辑器内执行导致打包游戏后字典空了
    //{
    //    dialogueIndex.Clear();
    //    foreach (var piece in dialoguePieces)
    //    {
    //        if (!dialogueIndex.ContainsKey(piece.ID))
    //            dialogueIndex.Add(piece.ID, piece);
    //    }
    //}
    void Awake()//保证在打包执行的游戏里第一时间获得对话的所有字典匹配 
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
                dialogueIndex.Add(piece.ID, piece);
        }
    }
    public QuestData_SO GetQuest()
    {
        QuestData_SO currentQuest=null;
        foreach(var piece in dialoguePieces)
        {
            if(piece.quest!=null)
                currentQuest=piece.quest;
        }
        return currentQuest;
    }
}

[System.Serializable]
public class DialoguePiece
{
    public string ID;
    public Sprite image;
    [TextArea]
    public string text;
    public QuestData_SO quest;
    public bool startFight;
    public int currentIndex;
    public List<DialogueOption> options = new List<DialogueOption>();

}

[System.Serializable]
public class DialogueOption
{
    [TextArea]
    public string text;
    public string targetID;
    public bool takeQuest;
}

