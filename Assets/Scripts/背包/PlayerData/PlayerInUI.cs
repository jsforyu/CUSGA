using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInUI : MonoBehaviour
{



    public CharacterData_SO PlayeyData;
    public Text Lv;
    public GameObject slot;
    public static PlayerInUI Instance;

    //public Text TiZhi;
    //public Text LiLiang;
    //public Text MinJie;
    //public Text FanYing;
    //public Text Left;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        slot.transform.GetChild(0).GetChild(0).GetComponent<ItemUI>().SetupItemUI(InventoryManager.instance.inventoryData.currentJianJi, InventoryManager.instance.inventoryData.currentJianJi.itemAmount);
        slot.transform.GetChild(1).GetComponent<Text>().text = "已装备剑技" + InventoryManager.instance.inventoryData.currentJianJi.itemName;
        slot.transform.GetChild(2).GetComponent<Text>().text =InventoryManager.instance.inventoryData.currentJianJi.description;
    }

    // Update is called once per frame
    void Update()
    {
        ShowText();
    }


    public void AddTi()
    {
        //if (PlayeyData != null && PlayeyData.剩余属性点 != 0)
        //{
        //    PlayeyData.体质++;
        //    PlayeyData.剩余属性点--;
        //}
    }
    public void AddLi()
    {
        //if (PlayeyData != null && PlayeyData.剩余属性点 != 0)
        //{
        //    PlayeyData.力量++;
        //    PlayeyData.剩余属性点--;
        //}
    }
    public void AddmMin()
    {
        //if (PlayeyData != null && PlayeyData.剩余属性点 != 0)
        //{
        //    PlayeyData.敏捷++;
        //    PlayeyData.剩余属性点--;
        //}
    }
    public void AddFan()
    {
        //if (PlayeyData != null && PlayeyData.剩余属性点 != 0)
        //{
        //    PlayeyData.反应++;
        //    //PlayeyData.剩余属性点--;
        //}
    }


    public void ShowText()
    {
        if(PlayeyData!=null)
            Lv.text = "当前等级："+PlayeyData.等级.ToString();
        //if (PlayeyData != null)
        //    TiZhi.text = "体质：" + PlayeyData.体质.ToString();
        //if (PlayeyData != null)
        //    LiLiang.text = "力量："+PlayeyData.力量.ToString();
        //if (PlayeyData != null)
        //    MinJie.text = "敏捷：" + PlayeyData.敏捷.ToString();
        //if (PlayeyData != null)
        //    FanYing.text = "反应：" + PlayeyData.反应.ToString();
        //if (PlayeyData != null)
            //Left.text = "剩余属性点：" + PlayeyData.剩余属性点.ToString();

    }
    
   


}
