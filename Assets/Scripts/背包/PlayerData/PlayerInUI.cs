using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInUI : MonoBehaviour
{



    public CharacterData_SO playerData;
    public Text Lv;
    public GameObject slot;
    public static PlayerInUI Instance;
    public Text attributeBody;

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

        attributeBody.text = string.Format(" 体质：          {0:D}\r\n 力量：          {1:D}\r\n 敏捷：          {2:D}\r\n 反应：          {3:D}\r\n 生命值：      {4:D}\r\n 架势条：      {5:D}\r\n 攻击力：      {6:D}\r\n 攻击次数：  {7:D}\r\n\r\n经验值： {8:D}/{9:D}", 
            playerData.体质, playerData.力量, playerData.敏捷, playerData.反应,
            (int)playerData.最大生命值, (int)playerData.最大架势条, (int)playerData.攻击力, playerData.挥刀次数,
            playerData.经验值, playerData.升级所需经验值);

        // slot.transform.GetChild(2).GetComponent<Text>().text =InventoryManager.instance.inventoryData.currentJianJi.description;
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
        if(playerData != null)
            Lv.text = "等级："+ playerData.等级.ToString();
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
