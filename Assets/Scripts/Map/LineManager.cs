using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    public Line[] Lines;//ÿһ��index��Ӧһ����
    public static LineManager Instance;
    public int lineindex;//ѡ�е��ߵı��
    public List<Line> linepoints;//ѡ�е�·��
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
         
            for(int i = 1; i < Lines.Length; i++)
        {
            Debug.Log("��һ��slot" + Lines[i].slot1.index + "�ڶ���slot" + Lines[i].slot2.index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindLine()
    {

    } 
}
