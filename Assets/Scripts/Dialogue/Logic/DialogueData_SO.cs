using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Dialogue",menuName ="Dialogue/DialogueData")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();
    //void OnValidate()//���ڱ༭����ִ�е��´����Ϸ���ֵ����
    //{
    //    dialogueIndex.Clear();
    //    foreach (var piece in dialoguePieces)
    //    {
    //        if (!dialogueIndex.ContainsKey(piece.ID))
    //            dialogueIndex.Add(piece.ID, piece);
    //    }
    //}
    void Awake()//��֤�ڴ��ִ�е���Ϸ���һʱ���öԻ��������ֵ�ƥ�� 
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

