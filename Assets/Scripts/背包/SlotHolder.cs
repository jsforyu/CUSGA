using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//设置背包 武器 盾牌格子  也可设其他的例如鞋子 头盔
public enum SlotType { BAG,WEAPON,ARMOR,ACTION,TRASH}
public class SlotHolder : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    //创建一个画布 起名Inventory Canvas 挂载Inventory Manager脚本 充当背包画布
    //在Inventory Canvas下创建一个图片起名 Inventory Bag 充当背包背景
    //在Inventory Bag下创建一个Panel管理所有格子 起名Inventory Contaniner 挂载Container UI 和Grid Layout Group组件
    //背包格子的创建：
    //创建一个Image 起名SlotHolder 挂载当前脚本 Image为背包格子的图片
    //在其子物体下创建一个空物体 起名 ItemSlot 挂载Item UI 和 Drag Item 脚本
    //ItemSlot子物体下创建图片 显示当前物品的图片 可以在图片下创建text 显示当前数量
    //将SlotHolder设为预制体 SlotHolder则为格子
    public InventoryData_SO inventoryData;  //背包数据库
    public InventoryData_SO equipmentData;
    public InventoryData_SO actionData;
    public SlotType slotType;
    public ItemUI itemUI;
    public GameObject tooptip;
    private void Update()
    {
        RefreshitemUI();
    }
    private void Awake()
    {
        tooptip = GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject;
        //ResfeshitemUI();
    }
    private void Start()
    {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }
    public void OnPointerEnter(PointerEventData eventData) //当鼠标移到当前格子时 更新道具说明栏的文本
    {
        if(itemUI.GetItem())
        {
            GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.SetupTooltip(itemUI.GetItem());
            GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData) //当鼠标移出则清空道具说明栏文本
    {
        GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
    }
      public void OnDisable() //被关闭时启用 防止直接关闭背包道具说明栏没隐藏
    {
        //GameObject.Find("Inventroy Canvas").GetComponent<InventoryManager>().tooltip.gameObject.SetActive(false);
        tooptip.SetActive(false);
    }
    public void RefreshitemUI()
    {
        if (itemUI.GetItem())
        {
            itemUI.SetupItemUI(itemUI.GetItem(), itemUI.GetItem().itemAmount);
        }
        else itemUI.gameObject.SetActive(false);
    }
}
