using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccessibleSkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData_SO skillData;
    public ItemUI itemUI;
    public GameObject tooltip;

    private void Awake()
    {
        tooltip = GameObject.Find("ToolTip");
    }

    public void InitItem(ItemData_SO skillData_)
    {
        skillData = skillData_;
        RefreshItemUI();
    }

    public void OnPointerEnter(PointerEventData eventData) //当鼠标移到当前格子时 更新道具说明栏的文本
    {
        tooltip.GetComponent<ItemTooltip>().SetupTooltip(skillData);
        tooltip.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData) //当鼠标移出则清空道具说明栏文本
    {
        tooltip.GetComponent<ItemTooltip>().SetupTooltip(null);
        tooltip.SetActive(false);
    }

    public void RefreshItemUI()
    {
        itemUI.SetupItemUI(skillData, skillData.itemAmount);
    }
}
