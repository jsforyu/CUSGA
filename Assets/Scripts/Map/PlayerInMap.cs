using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerInMap : Singleton<PlayerInMap>
{
    public int mapindex;//��ǰ���ڵĵ�ͼ�ڵ�
    public MapSlot curretnmapslot;
    public bool ismove;
    public float speed;
    public int currentindex;
    public PlayeInMapData data;
    public AudioSource walkingSound;

    int onfirst;
    Line online;
    Animator an;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayer();
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

    public void OnMove(Line line, int next)//���ڵ���ƶ������е�slot1���궼��slot2С
    {
        LineRenderer linere = line.GetComponent<LineRenderer>();
        MapSlot tomapslot;
        Debug.Log("��ʼ�ƶ�Э��");
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
    }
    public void Move(Line line, int next)
    {
        walkingSound.Play();
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
                walkingSound.Stop();
                mapindex = next;
                currentindex = 0;
                SaveManager.Instance.SavePlayerData();
            }
            else
            {
                yield return IE_MoveToTarget(linere.GetPosition(currentindex) + linere.transform.localPosition + linere.transform.parent.position);
                currentindex++;
            }
        }
        ismove = false;
        walkingSound.Stop();
        data.mapindex = mapindex;
        tomapslot.SlotFunction();
        SaveManager.Instance.SavePlayerData();
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
                walkingSound.Stop();
                mapindex = next;
                currentindex = 0;
                SaveManager.Instance.SavePlayerData();
            }
            else
            {
                yield return IE_MoveToTarget(linere.GetPosition(currentindex) + linere.transform.localPosition + linere.transform.parent.position);
                currentindex--;

            }

        }
        ismove = false;
        walkingSound.Stop();
        data.mapindex = mapindex;
        tomapslot.SlotFunction();
        SaveManager.Instance.SavePlayerData();
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


    void TurnPlyaer()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void LoadPlayer()
    {
        Debug.Log(data.mapindex);
        mapindex = data.mapindex;
        this.transform.position = MapManager.instance.Mapslots[mapindex].gameObject.transform.position;
        // �����������ĵ�ǰ�ڵ�Ϳɽ��룬��򿪽��밴ť
        MapManager.instance.Mapslots[mapindex].GetComponent<MapSlot>().SlotFunction();
    }

}
