using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ShowTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private ItemUI currentItemUI;
    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //QuestUI.Instance.tooltip.gameObject.SetActive(true);
        //QuestUI.Instance.tooltip.SetupTooltip(currentItemUI.currentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
