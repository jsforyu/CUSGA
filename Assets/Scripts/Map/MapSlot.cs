using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum MapSlotType
{
    City,
    Village,
    Outside
}
public class MapSlot : MonoBehaviour
{
    public GameObject tipUI;
    public MapSlotType Maptype;
    public int index;//�����
    public bool isstay;
    bool ischose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isstay && Input.GetMouseButtonDown(0)&&!PlayerInMap.Instance.ismove)
        {
            Debug.Log("��ʼһ��Ѱ·");

            StartCoroutine(FindSlot());//ʹ��Э��
            isstay = false;
        }
       
    }
    private void OnMouseEnter()
    {
        this.transform.localScale = new Vector3(2, 2, 1);
        isstay = true;
    }
    public void OnMouseExit()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        isstay = false;
    }

    public void SlotFunction()
    {



    }
    IEnumerator FindSlot()
    {
        FindSlotFir();
        List<int> realwaypoints=new List<int>();
        int first = index;//������·��
        while (first != PlayerInMap.Instance.mapindex)
        {
            realwaypoints.Add(first);
            first = MapManager.instance.waypoints[first];
        }
        realwaypoints.Reverse();
        first = PlayerInMap.Instance.mapindex;
        for(int i = 0; i < realwaypoints.Count; i++)
        {
            while (PlayerInMap.Instance.ismove)//��ɫ�����ƶ�
            {
                yield return null;
            }
            Line line = FindLine(first, realwaypoints[i]);
            PlayerInMap.Instance.Move(line, realwaypoints[i]);
            first = realwaypoints[i];
            Debug.Log(realwaypoints[i]);
        }
    }

    Line FindLine(int first,int next)
    {
       for(int i=1;i < LineManager.Instance.Lines.Length; i++)
        {
            if ((LineManager.Instance.Lines[i].slot1.index == first && LineManager.Instance.Lines[i].slot2.index == next)
                 || (LineManager.Instance.Lines[i].slot1.index == next && LineManager.Instance.Lines[i].slot2.index == first))
            {
                return LineManager.Instance.Lines[i];
            }
        }
        return null;
    }
    void FindSlotFir()//������ȱ����ҵ�,�ӵ�ǰ�㵽Ŀ�����������,���ؽ����һ����ļ���
    {
        Queue<int> q=new Queue<int>();
        bool[] visted=new bool[MapManager.instance.slotnumber];
        q.Enqueue(PlayerInMap.Instance.mapindex);
        visted[PlayerInMap.Instance.mapindex] = true;
        while (q.Count!=0)
        {
            int temp = q.Dequeue();
            
            if (temp == index)
            {
                //�ҵ��������
                return;
            }
            for(int i = 0; i < MapManager.instance.Slotslist[temp].Count; i++)
            {
                if (visted[MapManager.instance.Slotslist[temp][i]] == false)
                {
                    q.Enqueue(MapManager.instance.Slotslist[temp][i]);
                    visted[MapManager.instance.Slotslist[temp][i]] = true;
                    MapManager.instance.waypoints[MapManager.instance.Slotslist[temp][i]] = temp;
                } 
            }

        }
    }
}
