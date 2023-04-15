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
        slot.transform.GetChild(1).GetComponent<Text>().text = "��װ������" + InventoryManager.instance.inventoryData.currentJianJi.itemName;

        attributeBody.text = string.Format(" ���ʣ�          {0:D}\r\n ������          {1:D}\r\n ���ݣ�          {2:D}\r\n ��Ӧ��          {3:D}\r\n ����ֵ��      {4:D}\r\n ��������      {5:D}\r\n ��������      {6:D}\r\n ����������  {7:D}\r\n\r\n����ֵ�� {8:D}/{9:D}", 
            playerData.����, playerData.����, playerData.����, playerData.��Ӧ,
            (int)playerData.�������ֵ, (int)playerData.��������, (int)playerData.������, playerData.�ӵ�����,
            playerData.����ֵ, playerData.�������辭��ֵ);

        // slot.transform.GetChild(2).GetComponent<Text>().text =InventoryManager.instance.inventoryData.currentJianJi.description;
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
        if(playerData != null)
            Lv.text = "�ȼ���"+ playerData.�ȼ�.ToString();
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
