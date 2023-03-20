using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInMap : MonoBehaviour
{
    public int mapindex;//当前所在的地图节点
    public MapSlot curretnmapslot;
    public static PlayerInMap Instance;
    public bool ismove;
    public float speed;
    public int currentindex;
    int onfirst;
    Line online;
    Animator an;
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
        an = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ismove)
        {
            an.SetBool("walking", false);
        }
        else
        {
            an.SetBool("walking", true);
        }
    }

    public void OnMove(Line line, int next)//相邻点的移动，所有的slot1坐标都比slot2小
    {
        LineRenderer linere = line.GetComponent<LineRenderer>();
        MapSlot tomapslot;
        Debug.Log("开始移动协程");
        if (line.slot1.index == next)
        {
            tomapslot = line.slot1;
            if (transform.localScale.x > 0&&tomapslot.transform.position.x<transform.position.x)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            StartCoroutine(BackMove(tomapslot, linere, next));
        }
        else
        { 
            tomapslot = line.slot2;
            if (transform.localScale.x < 0 && tomapslot.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            StartCoroutine(ToMove(tomapslot, linere, next));
        }
        tomapslot.SlotFunction();
    }
    public void Move(Line line, int next)
    {
        ismove = true;
        online = line;
        onfirst = next;
        an.SetBool("walking", true);
        OnMove(online, onfirst);
    }
    IEnumerator ToMove(MapSlot tomapslot, LineRenderer linere,int next)
    {
        while (ismove && Vector3.Distance(this.transform.position, tomapslot.transform.position) > 0.0001f)
        {
            if (currentindex >= linere.positionCount)
            {
                yield return IE_MoveToTarget(tomapslot.gameObject.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, tomapslot.gameObject.transform.position, speed * Time.deltaTime);
                ismove = false;
                mapindex = next;
                currentindex = 0;
            }
            else
            {
                yield return IE_MoveToTarget(linere.GetPosition(currentindex) + linere.transform.localPosition + linere.transform.parent.position);
                currentindex++;
            }
        }
        ismove = false;
        yield return null;
    }
    IEnumerator BackMove(MapSlot tomapslot, LineRenderer linere, int next)
    {
        currentindex = linere.positionCount - 1;
        while (ismove && this.transform.position != tomapslot.transform.position)
        {
            if (currentindex <= 0)
            {
                yield return IE_MoveToTarget(tomapslot.gameObject.transform.position);
                ismove = false;
                mapindex = next;
                currentindex = 0;
            }
            else
            {
                yield return IE_MoveToTarget(linere.GetPosition(currentindex) + linere.transform.localPosition + linere.transform.parent.position);
                currentindex--;

            }

        }
        ismove = false;
        yield return null;
    }

    /// <summary>
    /// 将自己移动到目标点
    /// </summary>
    /// <param name="target">目标点</param>
    /// <returns></returns>
    IEnumerator IE_MoveToTarget(Vector3 target)
    {
        while (Vector3.Distance(this.transform.position, target) > 0.0001f)
        {
            Vector3 nextStep = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.position = nextStep;
            //Debug.Log(nextStep + ":" + target);
            yield return null;
        }
    }


    void TurnPlyaer()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
