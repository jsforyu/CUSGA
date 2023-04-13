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
        slot.transform.GetChild(1).GetComponent<Text>().text = "��װ������" + InventoryManager.instance.inventoryData.currentJianJi.itemName;
        slot.transform.GetChild(2).GetComponent<Text>().text =InventoryManager.instance.inventoryData.currentJianJi.description;
    }

    // Update is called once per frame
    void Update()
    {
        ShowText();
    }


    public void AddTi()
    {
        //if (PlayeyData != null && PlayeyData.ʣ�����Ե� != 0)
        //{
        //    PlayeyData.����++;
        //    PlayeyData.ʣ�����Ե�--;
        //}
    }
    public void AddLi()
    {
        //if (PlayeyData != null && PlayeyData.ʣ�����Ե� != 0)
        //{
        //    PlayeyData.����++;
        //    PlayeyData.ʣ�����Ե�--;
        //}
    }
    public void AddmMin()
    {
        //if (PlayeyData != null && PlayeyData.ʣ�����Ե� != 0)
        //{
        //    PlayeyData.����++;
        //    PlayeyData.ʣ�����Ե�--;
        //}
    }
    public void AddFan()
    {
        //if (PlayeyData != null && PlayeyData.ʣ�����Ե� != 0)
        //{
        //    PlayeyData.��Ӧ++;
        //    //PlayeyData.ʣ�����Ե�--;
        //}
    }


    public void ShowText()
    {
        if(PlayeyData!=null)
            Lv.text = "��ǰ�ȼ���"+PlayeyData.�ȼ�.ToString();
        //if (PlayeyData != null)
        //    TiZhi.text = "���ʣ�" + PlayeyData.����.ToString();
        //if (PlayeyData != null)
        //    LiLiang.text = "������"+PlayeyData.����.ToString();
        //if (PlayeyData != null)
        //    MinJie.text = "���ݣ�" + PlayeyData.����.ToString();
        //if (PlayeyData != null)
        //    FanYing.text = "��Ӧ��" + PlayeyData.��Ӧ.ToString();
        //if (PlayeyData != null)
            //Left.text = "ʣ�����Ե㣺" + PlayeyData.ʣ�����Ե�.ToString();

    }
    
   


}
