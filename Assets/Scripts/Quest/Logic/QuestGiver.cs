using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    MapSlot controller; 
    QuestData_SO currentQuest;
    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishDialogue;
    public bool IsStarted
    {
        get 
        { 
            if (QuestManager.Instance.HaveQuest(currentQuest))
               { return QuestManager.Instance.GetTask(currentQuest).IsStarted; } 
            else return false;
        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            { return QuestManager.Instance.GetTask(currentQuest).IsComplete; }
            else return false;
        }
    }
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            { return QuestManager.Instance.GetTask(currentQuest).IsFinished; }
            else return false;
        }
    }
    private void Awake()
    {
        controller = GetComponent<MapSlot>();
    }
    private void Start()
    {
        controller.currentData = startDialogue;
        currentQuest = controller.currentData.GetQuest();
    }
    private void Update()
    {
        if(IsStarted)
        {
            if (IsComplete)
            {
                controller.currentData = completeDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }
        if(IsFinished)
        {
            controller.currentData = finishDialogue;
        }
    }
}
