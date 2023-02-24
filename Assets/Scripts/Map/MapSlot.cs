using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MapSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{

    public GameObject HighlightObject;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
            HighlightObject.SetActive(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

            HighlightObject.SetActive(false);

       
    }
}
