using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ContainerUI : MonoBehaviour
{
    //�����и��Ӹ�ֵ��� ʡȥ�ֶ���Inspector��ֵ����
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
        for(int i=0;i< InventoryManager.instance.inventoryData.items.Count; i++)//�����ڵ����и��ӱ��

        {
            //slotHolders[i].itemUI.Index = i;
        }
        
    }

    public void RefreshSlot()
    {
        if (this.transform.childCount > 0)//ɾ�����и���
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }
        for (int i=0;i< InventoryManager.instance.inventoryData.items.Count; i++)//�������ɸ���
        {
           GameObject slotholder=Instantiate(slot, this.transform);
            slotholder.GetComponent<SlotHolder>().itemUI.Index = i;//����ֵ
            slotHolders.Add(slotholder.GetComponent<SlotHolder>());
            
        }
    }
}
