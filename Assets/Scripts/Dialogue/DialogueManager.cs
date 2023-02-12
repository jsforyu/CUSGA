using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : Singleton<DialogueManager>
{
    [Header("Basic Elements")]
    public Image icon;
    public Text chatText;
    [Header("Data")]
    public DialogueData_SO currentData;
    int currentID = 0;
    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentID = 0;
    }
}
