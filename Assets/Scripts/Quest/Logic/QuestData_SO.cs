using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName ="New Quest",menuName ="Quest/QuestData")]
public class QuestData_SO : ScriptableObject
{
    [Header("任务名字")]
    public string questName;
    [Header("任务描述")]
    [TextArea]
    public string description;

    public bool isStarted;
    public bool isComplete;
    public bool isFinished;
    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards=new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isComplete = finishRequires.Count() == questRequires.Count;
    }
    public void GiveRewards()
    {
        foreach (var reward in rewards) 
        {
            if(reward.amount<0)
            {
                int requireCount = Mathf.Abs(reward.amount);
                if(InventoryManager.instance.QuestItemInBag(reward.ItemData)!=null)
                {
                    if(InventoryManager.instance.QuestItemInBag(reward.ItemData).amount<=requireCount)
                    {
                        requireCount -= InventoryManager.instance.QuestItemInBag(reward.ItemData).amount;
                        InventoryManager.instance.QuestItemInBag(reward.ItemData).amount = 0;
                        if(InventoryManager.instance.QuestItemInAction(reward.ItemData)!=null)
                        {
                            InventoryManager.instance.QuestItemInAction(reward.ItemData).amount -= requireCount;
                        }
                    
                    }
                    else
                    {
                        InventoryManager.instance.QuestItemInBag(reward.ItemData).amount -= requireCount;
                    }
                }
                else
                {
                    InventoryManager.instance.QuestItemInAction(reward.ItemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.instance.inventoryData.AddItem(reward.ItemData, reward.amount);
            }
            InventoryManager.instance.inventoryUI.RefreshUI();
            InventoryManager.instance.actionUI.RefreshUI();
        }
    }
    public List<string> RequireTargetName()
    {
        List<string> targetNameList = new List<string>();
        foreach (var require in questRequires) 
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
}
[System.Serializable]
public class QuestRequire
{
    public string name;
    public int requireAmount;
    public int currentAmount;
}


