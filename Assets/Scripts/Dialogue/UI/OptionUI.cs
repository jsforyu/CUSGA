using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    private Button thisButton;
    private DialoguePiece currentPiece;
    private bool takeQuest;
    private string nextPieceID;
    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }
    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentPiece= piece;
        optionText.text=option.text;
        nextPieceID = option.targetID;
        takeQuest = option.takeQuest; 
    }
    public void OnOptionClicked()
    {
        if(currentPiece.quest!=null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)
            };
            if(takeQuest)
            {
                if(QuestManager.Instance.HaveQuest(newTask.questData))
                {

                }
                else
                {
                    QuestManager.Instance.tasks.Add(newTask);
                }
            }
        }
        if (nextPieceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }

    }
}
