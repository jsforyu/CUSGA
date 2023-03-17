using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInMap : MonoBehaviour
{
    public int mapindex;//��ǰ���ڵĵ�ͼ�ڵ�
    public MapSlot curretnmapslot;
    public static PlayerInMap Instance;
    public bool ismove;
    public float speed;
    public int currentindex;
    int onfirst;
    Line online;
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
       
    }

    public void OnMove(Line line, int next)//���ڵ���ƶ������е�slot1���궼��slot2С
    {
        LineRenderer linere = line.GetComponent<LineRenderer>();
        MapSlot tomapslot;
        Debug.Log("��ʼ�ƶ�Э��");
        if (line.slot1.index == next)
        {
            tomapslot = line.slot1;
            StartCoroutine(BackMove(tomapslot, linere, next));
        }
        else
        { 
            tomapslot = line.slot2;
            StartCoroutine(ToMove(tomapslot, linere, next));
        }     
    }
    public void Move(Line line, int next)
    {
        ismove = true;
        online = line;
        onfirst = next;
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
    /// ���Լ��ƶ���Ŀ���
    /// </summary>
    /// <param name="target">Ŀ���</param>
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
}
