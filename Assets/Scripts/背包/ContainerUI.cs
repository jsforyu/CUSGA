using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ContainerUI : MonoBehaviour
{
    //�����и��Ӹ�ֵ��� ʡȥ�ֶ���Inspector��ֵ����
    //�̶���������
    public List<SlotHolder> slotHolders=new List<SlotHolder>();
    public GameObject slot; 
    public void Update()
    {
        //RefreshUI();
        //RefreshSlot();
    }
    private void Awake()
    {
        RefreshSlot();//��ʼ���ɸ���
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
        for (int i=0;i< InventoryManager.instance.inventoryData.items.Count; i++)//�������ɸ���
        {
           GameObject slotholder=Instantiate(slot, this.transform);
            slotholder.GetComponent<SlotHolder>().itemUI.Index = i;//����ֵ
            slotHolders.Add(slotholder.GetComponent<SlotHolder>());
            
        }
    }
}
