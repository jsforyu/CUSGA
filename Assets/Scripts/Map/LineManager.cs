using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    public List<Line> Lines;
    public static LineManager Instance;
    public int lineindex;//ѡ�е��ߵı��
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
         
            for(int i = 0; i < Lines.Count; i++)
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
