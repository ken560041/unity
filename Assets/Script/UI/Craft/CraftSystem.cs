using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSystem : MonoBehaviour
{


    public static CraftSystem Instance { get; set; }

    public GameObject craftSceenUI;

    public List<GameObject>slotList=new List<GameObject>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquit;


    public bool isOpen;

    // Start is called before the first frame update

    List<Recipe> recipeList= new List<Recipe>
        {
             new Recipe("wood_log", "2xtwig_green, 1xstoune"),
             new Recipe("Armor", "2xtwig_green, 1xstoune, 3xmushroom_big_red"),

        }; 


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
        LoadItemImage();
    }

    // Update is called once per frame
    void Update()
    {
       if( !isOpen && Input.GetKeyDown(KeyCode.C) )
        {
            craftSceenUI.SetActive(true);
            isOpen = true;
        }
       else if( isOpen && Input.GetKeyDown(KeyCode.C) ) {

            craftSceenUI.SetActive(false);
            isOpen = false;
        }
        
    }
    public Recipe FindRecipe(string itemName)
    {
        return recipeList.Find(recipe => recipe.ItemName == itemName);
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
        PopulateSlotListRecursive(craftSceenUI.transform);
       
    }


    private void LoadItemImage()
    {
        AddInSystemCraft("Armor");
        AddInSystemCraft("wood_log");
        AddInSystemCraft("pickaxe_basic");
        AddInSystemCraft("sword");
        AddInSystemCraft("weapon_icon");
        AddInSystemCraft("shield_basic_metal");
        AddInSystemCraft("bottle_standard_blue");
        AddInSystemCraft("bottle_standard_green");
        AddInSystemCraft("necklace_siver_red");
        AddInSystemCraft("stone_blocks_grey");
        AddInSystemCraft("arrow_basic");
        AddInSystemCraft("bow_wood1");
        InventorySystem.Instance.AddToInventoryAdmin("shield_basic_metal", 30);
        InventorySystem.Instance.AddToInventoryAdmin("twig_green", 10);
        InventorySystem.Instance.AddToInventoryAdmin("stoune", 30);
        InventorySystem.Instance.AddToInventoryAdmin("mushroom_big_red", 30);
    }


    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject child in slotList)
        {
            if (child.transform.childCount == 0)
            {
                return child;
            }
        }
        return new GameObject();
    }



    private void AddInSystemCraft(string itemName)
    {
        whatSlotToEquit=FindNextEmptySlot();
        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquit.transform.position, whatSlotToEquit.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquit.transform);
        itemToAdd.name = itemName;
        // Lấy RectTransform của itemToAdd
        RectTransform rectTransform = itemToAdd.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            // Đặt kích thước cụ thể cho RectTransform
            rectTransform.sizeDelta = new Vector2(50, 50); // width = 50, height = 50

            // Đặt vị trí và scale lại cho phù hợp
            rectTransform.anchoredPosition = Vector2.zero; // Đặt về giữa của cha
            rectTransform.localScale = Vector3.one; // Đặt scale về 1 để không bị thay đổi kích thước bởi cha
        }
        else
        {
            Debug.LogError("itemToAdd không có RectTransform.");
        }
        //Debug.Log("Parent của itemToAdd: " + itemToAdd.transform.parent.name);
    }
    public List<GameObject> GetSlotList()
    {
        return slotList;
    }



    public int CountCompumentObject( string nameCt)
    {
        Recipe recipe =FindRecipe(nameCt);
        int count = 0;
        if (recipe != null)
        {
            foreach(KeyValuePair<string,int> ingredient in recipe.Ingredients)
            {
                string ingredientName = ingredient.Key; // Tên nguyên liệu
                int ingredientQuantity = ingredient.Value; // Số lượng nguyên liệu
                                                           // print 

                count++;
                //DrawOverivederCT.Instance.setImageCompument()



            }

          //  Debug.Log(count);
        }
        return count;

    }

    public bool CamCraft(string nameItemCf)
    {
        Recipe recipe=FindRecipe(nameItemCf);
        if(recipe != null)
        {
            foreach(KeyValuePair<string,int>ingredient in recipe.Ingredients)
            {
                string ingredientName = ingredient.Key; // Tên nguyên liệu
                int ingredientQuantity = ingredient.Value;

                bool checkName= !InventorySystem.Instance.CheckItemExits(ingredientName);
                bool checkCount=InventorySystem.Instance.returnCountItem(ingredientName)<ingredientQuantity;
                Debug.Log(InventorySystem.Instance.returnCountItem(ingredientName));
                if (checkName || checkCount) {
                    return false;
                }


                
            }
        }

        return true;
    }


    public void CraftItemNew()
    {

        string itemNeedCraft = DrawOverivederCT.Instance.returnNameCt();
        if (CamCraft(itemNeedCraft))
        {
            Recipe recipe=FindRecipe(itemNeedCraft);
            if(recipe != null)
            {
                foreach(KeyValuePair<string,int> ingredient in recipe.Ingredients)
                {
                    string ingredientName = ingredient.Key; // Tên nguyên liệu
                    int ingredientQuantity = ingredient.Value;

                     InventorySystem.Instance.UpdateInvatoryBag(ingredientName, ingredientQuantity);
                }
            }
            InventorySystem.Instance.AddListItem(itemNeedCraft);
            InventorySystem.Instance.UpdateAfterCraft();
        }
    }

   
}
