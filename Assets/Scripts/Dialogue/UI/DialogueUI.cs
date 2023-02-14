using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public Image icon;        
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;
    [Header("Options")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;
    [Header("Data")]
    public DialogueData_SO currentData;
    int currentIndex = 0;
    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }
    void ContinueDialogue()
    {
        if (currentIndex < currentData.dialoguePieces.Count)
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        else dialoguePanel.SetActive(false);
    }
    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
    }
    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else icon.enabled = false;
        mainText.text = "";
        mainText.text = piece.text;
        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.gameObject.SetActive(true);
            currentIndex++;
        }
        else nextButton.gameObject.SetActive(false);
        CreateOptions(piece);
    }
    void CreateOptions(DialoguePiece piece)
    {
        if(optionPanel.childCount>0)
        {
            for(int i=0;i<optionPanel.childCount;i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }
        //for(int i=0;i<length;i++)
        //{

        //}
    }
}
