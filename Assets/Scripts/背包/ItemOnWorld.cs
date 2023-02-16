using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemData_SO item;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InventoryManager.instance.inventoryData.AddItem(item, 1);
            Destroy(gameObject);
        }
    }
}
