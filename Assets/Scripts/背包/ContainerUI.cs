using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ContainerUI : MonoBehaviour
{
    //给所有格子赋值编号 省去手动在Inspector赋值步骤
    public List<SlotHolder> slotHolders=new List<SlotHolder>();
    public GameObject slot; 
    public void Update()
    {
        //RefreshUI();
        //RefreshSlot();
    }
    private void Awake()
    {
        //RefreshSlot();
    }
    private void OnEnable()
    {
        
    }
    public void RefreshUI()
    {
        for(int i=0;i< InventoryManager.instance.inventoryData.items.Count; i++)//给存在的所有格子编号

        {
            //slotHolders[i].itemUI.Index = i;
        }
        
    }

    public void RefreshSlot()
    {
        if (this.transform.childCount > 0)//删除所有格子
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }
        for (int i=0;i< InventoryManager.instance.inventoryData.items.Count; i++)//重新生成格子
        {
           GameObject slotholder=Instantiate(slot, this.transform);
            slotholder.GetComponent<SlotHolder>().itemUI.Index = i;//给赋值
            slotHolders.Add(slotholder.GetComponent<SlotHolder>());
            
        }
    }
}
