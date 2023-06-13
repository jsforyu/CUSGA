using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class ItemTooltip : MonoBehaviour
{
    //���ǵ���˵���� ������Ƶ������ϻ��е��ߵ���ϸ˵��
    //ʹ�÷���������һ��text UpdatePosition��text��λ�ø������ SetupTooltip�� 
    public Text itemNameText;
    public Text itemInfoText;
    RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetupTooltip(ItemData_SO item)
    {
        if (item != null)
        {
            itemNameText.text = item.itemName;
            itemInfoText.text = item.description + "\r\n\r\n" + item.effect;
        }
        else
        {
            itemNameText.text = "";
            itemInfoText.text = "";
        }
    }
    public void SetupTooltipWithDescription(ItemData_SO item)
    {
        if (item != null)
        {
            itemNameText.text = item.itemName;
            itemInfoText.text = item.description + "\r\n\r\n" + item.effect;
        }
        else
        {
            itemNameText.text = "";
            itemInfoText.text = "";
        }
    }
    void OnEnable()
    {
        UpdatePosition();
    }
    private void Update()
    {
        UpdatePosition();
    }
    public void UpdatePosition()  //�������λ���ƶ�
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector3[] corners=new Vector3[4]; 
        //rectTransform.GetWorldCorners(corners);
        //float width = corners[3].x - corners[0].x;
        //float height=corners[1].y-corners[0].y;
        //if (mousePos.y < height)
        //    rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        //else if (Screen.width - mousePos.x > width)
        //    rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        //else
        //    rectTransform.position = mousePos + Vector3.left * width * 0.6f;

        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mousePos = Input.mousePosition;

        // �����λ�ô���Ļ����ת��Ϊ��������
        //mousePos = Camera.main.ScreenToViewportPoint(mousePos);

        // ��UIԪ�ص�λ������Ϊ���λ��
        rectTransform.position = mousePos;
    }
}
