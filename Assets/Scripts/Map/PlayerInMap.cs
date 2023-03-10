using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class PlayerInMap : MonoBehaviour
{
    public int mapindex;//当前所在的地图节点
    public MapSlot tomapslot;//要去的地图节点
    public static PlayerInMap Instance;
    public bool ismove;
    public float speed;
    public int currentindex;
    public PathCreator pathcreater;
    Queue<Vector3> potion = new Queue<Vector3>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentindex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
    }

    public void OnMove()//相邻点的移动
    {
        LineRenderer linere = LineManager.Instance.Lines[LineManager.Instance.lineindex].GetComponent<LineRenderer>();
        if (ismove&&this.transform.position!=tomapslot.transform.position)
        {
            if (currentindex >= linere.positionCount)
            {
                transform.position=Vector3.MoveTowards(transform.position,tomapslot.transform.position,speed*Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, linere.GetPosition(currentindex), speed * Time.deltaTime); 
                currentindex++;
            }
            
        }
        if(ismove&&this.transform.position == tomapslot.transform.position)
        {
            mapindex = tomapslot.index;
            ismove = false;
            currentindex = 0;
        }
    }
}
