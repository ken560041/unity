using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;
    public bool playerInRange;
    public string GetItemName()
    {
        return ItemName;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange && SelectionManager.Instance.onTarget&& SelectionManager.Instance.selectObject==gameObject)
        {
            /*Debug.Log("iteem addd to inventory");   
            Destroy(gameObject);
*/

            if (!InventorySystem.Instance.CheckFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("isVantory full");
            }
        }  
    }
}