using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public MapSlot[] Mapslots;//����ļ���,һ��index��Ӧһ��mapslot
    public Dictionary<int, List<int>> Slotslist=new Dictionary<int, List<int>>();//�ڽӱ�
    public int[] waypoints;
    public static MapManager instance;
    public int slotnumber;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        waypoints = new int[slotnumber];
        CreatList();
        for(int i = 0; i < slotnumber; i++)
        {
            Debug.Log(i + "�ڵ�" + Mapslots[i].gameObject.transform.position);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreatList()//����·�ߴ����ڽӱ�
    {
        //��ʼ���ڽӱ�
        for(int i = 0; i < Mapslots.Length; i++)
        {
            List<int> slotindex = new List<int>();
            Slotslist.Add(i, slotindex);
        }
        for(int i = 0; i < LineManager.Instance.Lines.Count; i++)
        {
            Slotslist[LineManager.Instance.Lines[i].slot1.index ].Add(LineManager.Instance.Lines[i].slot2.index);
            Slotslist[LineManager.Instance.Lines[i].slot2.index].Add(LineManager.Instance.Lines[i].slot1.index);
        }
    }
}
