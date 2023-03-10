using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapSlot : MonoBehaviour
{
    public GameObject tipUI;
    public int index;//结点编号
    bool isstay;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isstay && Input.GetMouseButtonDown(0))
        {
            FindSlot();
            isstay = false;
        }
    }
    private void OnMouseEnter()
    {
        isstay = true;
        this.transform.localScale = new Vector3(2, 2, 1);

    }
    private void OnMouseExit()
    {
        isstay = false;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    void SlotFunction()
    {

    }
    public void FindSlot()
    {
        for (int i = 0; i < LineManager.Instance.Lines.Count; i++) 
        {
            if ((LineManager.Instance.Lines[i].slot1.index == PlayerInMap.Instance.mapindex && LineManager.Instance.Lines[i].slot2.index == this.index)
                  || (LineManager.Instance.Lines[i].slot1.index == this.index && LineManager.Instance.Lines[i].slot2.index == PlayerInMap.Instance.mapindex))
            {
                //找到这条线
                LineManager.Instance.lineindex = i;
                PlayerInMap.Instance.ismove=true;
                FindDes();
                break;
            }
        }
    }

    void FindDes()
    {
        if (LineManager.Instance.Lines[LineManager.Instance.lineindex].slot1.index == PlayerInMap.Instance.mapindex)
        {
            PlayerInMap.Instance.tomapslot = LineManager.Instance.Lines[LineManager.Instance.lineindex].slot2;
        }
        if (LineManager.Instance.Lines[LineManager.Instance.lineindex].slot2.index == PlayerInMap.Instance.mapindex)
        {
            PlayerInMap.Instance.tomapslot = LineManager.Instance.Lines[LineManager.Instance.lineindex].slot1;
        }
    }
}
