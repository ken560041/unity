using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStruct 
{
    // Start is called before the first frame update
    public string ItemName;
    public int CountItem;
    public ItemStruct(string itemName, int countItem)
    {
        ItemName = itemName;
        CountItem = countItem;
    }

}
