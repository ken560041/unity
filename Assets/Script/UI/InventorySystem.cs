using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;




    public List<GameObject>slotList=new List<GameObject>();

    public List<string> slotNames = new List<string>();


    public List<ItemStruct> listItem = new List<ItemStruct>(); 

    private GameObject itemToAdd;
    private GameObject whatSlotToEquit;


    public bool isOpen;

/*    public bool isFull;*/

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        
        PopulateSlotList();
    }


    private void PopulateSlotListRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }

            // Duyệt tiếp vào cấp con
            PopulateSlotListRecursive(child);
        }
    }

    // Gọi hàm với Transform cha
    private void PopulateSlotList()
    {
        slotList.Clear(); // Xóa danh sách cũ (nếu có)
        PopulateSlotListRecursive(inventoryScreenUI.transform);
    }



    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {
      
       // Debug.Log(CheckItemExits(itemName));
        if (!CheckItemExits(itemName))
        {
            ItemStruct tempItem = new ItemStruct(itemName, 1);
            // neu chua co item nay trong 
            whatSlotToEquit = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquit.transform.position, whatSlotToEquit.transform.rotation);
            itemToAdd.name = itemName;
            itemToAdd.transform.SetAsFirstSibling();
            CreateText(whatSlotToEquit, "x1", new Vector2(0, -15), 10, Color.white);
            itemToAdd.transform.SetParent(whatSlotToEquit.transform);
            // Debug.Log("Parent của itemToAdd: " + itemToAdd.transform.parent.name);s
            slotNames.Add(itemName);
            listItem.Add(tempItem);
        }
        else
        {
            updateCountItem(itemName,1);
            GameObject temp=FindItem(itemName);
            if (temp != null)
            {
                foreach (Transform tmp in  temp.transform)
                {
                    if (tmp.name!= itemName) {
                        Debug.Log(tmp.name);
                        Text textComponent = tmp.GetComponentInChildren<Text>(); // Nếu là UI Text
                        if (textComponent != null)
                        {
                            string result = $"x{returnCountItem(itemName)}";
                            textComponent.text = result;
                            Debug.Log(result);
                        }
                        
                        
                    }
                }
            }
            
            //CreateText(temp, result, new Vector2(0, -15), 10, Color.white);
        }
        
       
    }


    public void AddToInventoryAdmin(string name, int count )
    {
        if (!CheckItemExits(name))
        {
            ItemStruct tempItem = new ItemStruct(name, count);
            listItem.Add(tempItem);
            whatSlotToEquit = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(name), whatSlotToEquit.transform.position, whatSlotToEquit.transform.rotation);
            itemToAdd.name = name;
            itemToAdd.transform.SetAsFirstSibling();
            string result = $"x{returnCountItem(name)}";
            CreateText(whatSlotToEquit, result, new Vector2(0, -15), 10, Color.white);
            itemToAdd.transform.SetParent(whatSlotToEquit.transform);
            // Debug.Log("Parent của itemToAdd: " + itemToAdd.transform.parent.name);s
            slotNames.Add(name);
            
        }
        else
        {

            Debug.Log("thuc hien cai nay");
            updateCountItem(name, count);
            GameObject temp = FindItem(name);
            if (temp != null)
            {
                foreach (Transform tmp in temp.transform)
                {
                    if (tmp.name != name)
                    {
                        Debug.Log(tmp.name);
                        Text textComponent = tmp.GetComponentInChildren<Text>(); // Nếu là UI Text
                        if (textComponent != null)
                        {
                            string result = $"x{returnCountItem(name)}";
                            textComponent.text = result;
                        }


                    }
                }
            }
        }
    }

    public void PrintfInventoryUpdateAfter(string name, int count)
    {
        whatSlotToEquit = FindNextEmptySlot();
        itemToAdd = Instantiate(Resources.Load<GameObject>(name), whatSlotToEquit.transform.position, whatSlotToEquit.transform.rotation);
        itemToAdd.name = name;
        itemToAdd.transform.SetAsFirstSibling();
        string result = $"x{returnCountItem(name)}";
        CreateText(whatSlotToEquit, result, new Vector2(0, -15), 10, Color.white);
        itemToAdd.transform.SetParent(whatSlotToEquit.transform);
    }


    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount==0)
            {
                return slot;
                Debug.Log("co ton tai");
            }
        }
        return new GameObject();
    }

    private GameObject FindItem(string name)
    {
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount != 0)
            {
                for(int i=0;  i<slot.transform.childCount; i++)
                {
                    if(slot.transform.GetChild(i).name == name)
                    {
                        return slot;
                    }
                }
            }
        }
        return null;
    }


    public bool CheckFull()
    {
        int counter = 0;
        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount>0)
            {
                counter++;
                Debug.Log(slot.name);
            }
            
        }

        if (counter == 21)
        {
            return true;
        }
        else
        
        {
            
            return false;
        }

  
    }

    public void CreateText(GameObject parent, string textContent, Vector2 anchoredPosition, int fontSize, Color color)
    {
        // Kiểm tra xem cha có thuộc Canvas không
        if (parent.GetComponentInParent<Canvas>() == null)
        {
            Debug.LogError("Parent object must be under a Canvas!");
            return;
        }

        // Tạo GameObject mới cho Text
        GameObject textObject = new GameObject("Text(txp)");
        textObject.transform.SetParent(parent.transform, false); // Đặt cha là parent
        textObject.transform.SetAsLastSibling();
        // Thêm RectTransform (tự động thêm khi sử dụng UI Text)
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();

        // Thiết lập RectTransform
        rectTransform.anchoredPosition = anchoredPosition; // Vị trí so với cha
        rectTransform.sizeDelta = new Vector2(50, 20);    // Kích thước mặc định
        rectTransform.localScale = Vector3.one;           // Đặt tỉ lệ mặc định

        // Thêm thành phần Text
        Text text = textObject.AddComponent<Text>();
        text.text = textContent;                         // Nội dung của Text
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); // Sử dụng font mặc định Arial
        text.fontSize = fontSize;                        // Kích thước chữ
        text.color = color;                              // Màu chữ
        text.alignment = TextAnchor.LowerRight;



        //Debug.Log($"Text object '{textObject.name}' created under '{parent.name}'.");
    }


    public bool CheckItemExits(string itemName)
    {
        foreach(ItemStruct item in listItem)
        {
            if(item.ItemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void updateCountItem(string itemName, int add)
    {
        foreach (ItemStruct item in listItem)
        {
            if (item.ItemName == itemName)
            {
                item.CountItem+=add;
            }
        }
    }

    public int returnCountItem(string itemName) {
        foreach (ItemStruct item in listItem)
        {
            if (item.ItemName == itemName)
            {
                return item.CountItem;
            }
        }
        return 0;
    }



    public bool CheckSlotName(string itemName)
    {
        foreach (String itemSlotName in slotNames)
        {
            if (itemSlotName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateInvatoryBag(string name, int count)
    {
        foreach (ItemStruct item in listItem)
        {
            if (item.ItemName == name)
            {
                item.CountItem -= count;
            }
        }
    }
    public void RemoveItemsWithZeroCount()
    {
        listItem.RemoveAll(item => item.CountItem == 0);
    }

    public void ClearnSlotList()
    {
        foreach (GameObject temp in slotList)
        {
            if (temp.transform.childCount!=0) {
                ClearChildObjects(temp);
            }
        }
    }

    public void ClearChildObjects(GameObject parent)
    {
        if (parent == null)
        {
            //Debug.LogWarning("Parent object is null. Cannot clear child objects.");
            return;
        }

        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

       // Debug.Log($"Cleared all child objects of {parent.name}");
    }


    public void UpdateAfterCraft()
    {
        ClearnSlotList();
        RemoveItemsWithZeroCount();
        foreach (ItemStruct item in listItem)
        {
            PrintfInventoryUpdateAfter(item.ItemName, item.CountItem);
            Debug.Log(item.ItemName + "     " + item.CountItem);
        }
    }


    public void AddListItem(string nameItem)
    {
        foreach(ItemStruct item in listItem)
        {
            if(item.ItemName == nameItem)
            {
                item.CountItem++;
                return;
            }

        }
        AddToInventoryAdmin(nameItem, 1);

    }
}