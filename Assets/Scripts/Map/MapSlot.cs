using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapSlot : MonoBehaviour
{
    public GameObject tipUI;
    public GameObject HighlightObject;
    bool isstay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HighLight()
    {
       
    }
    private void OnMouseEnter()
    {
        this.transform.localScale = new Vector3(2, 2, 1);

    }
    private void OnMouseExit()
    {
        this.transform.localScale = new Vector3(1, 1, 1);

    }
}
