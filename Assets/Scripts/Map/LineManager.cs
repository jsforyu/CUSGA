using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    public List<Line> Lines;
    public static LineManager Instance;
    public int lineindex;//选中的线的编号
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
            Debug.Log("第一个slot" + Lines[i].slot1.index + "第二个slot" + Lines[i].slot2.index);
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
