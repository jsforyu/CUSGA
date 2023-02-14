using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ContainerUI : MonoBehaviour
{
    //给所有格子赋值编号 省去手动在Inspector赋值步骤
      public SlotHolder[] slotHolders;
    public void Update()
    {
        RefreshUI();
    }
    public void RefreshUI()
    {
        for(int i=0;i<slotHolders.Length;i++)

        {
            slotHolders[i].itemUI.Index = i;
        }
    }
}
