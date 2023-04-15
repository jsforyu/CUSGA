using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class ItemTooltip : MonoBehaviour
{
    //这是道具说明栏 当鼠标移到道具上会有道具的详细说明
    //使用方法：创建一个text UpdatePosition将text的位置跟随鼠标 SetupTooltip在 
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
    public void UpdatePosition()  //跟着鼠标位置移动
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

        // 获取鼠标在屏幕上的位置
        Vector3 mousePos = Input.mousePosition;

        // 将鼠标位置从屏幕坐标转换为世界坐标
        //mousePos = Camera.main.ScreenToViewportPoint(mousePos);

        // 将UI元素的位置设置为鼠标位置
        rectTransform.position = mousePos;
    }
}
