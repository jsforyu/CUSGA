using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;
    public QuestData_SO currentData;
    public Text questContentText;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);

    }
    void UpdateQuestContent()
    {
        questContentText.text = currentData.description;
        QuestUI.Instance.SetUpRequireList(currentData);
        foreach(Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }
        foreach(var item in currentData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.ItemData, item.amount);   
        }
    }
    public void SetupNameButton(QuestData_SO questData)
    {
        currentData=questData;
        if(questData.isComplete)
        {
            questNameText.text = questData.questName + "(���)";
        }
        else
        {
            questNameText.text=questData.questName;
        }
    }
}
