using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDataCraft : MonoBehaviour
{
    // Start is called before the first frame update


    CraftSystem craftSystem;

    public GameObject buttonObject;
    public string imageName; // Tên ảnh
    public List<GameObject> childObjects; // Các đối tượng con

    public GameObject targetUI; // UI đích

    public Transform targetListParent; // Cha của danh sách đối tượng con
    public GameObject listItemPrefab; // Prefab cho danh sách con
    void Start()
    {
        buttonObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnButtonClick()
    {
        
        GameObject test= FindNextObject();
        GameObject test1=test.transform.GetChild(0).gameObject;
       // Debug.Log(test.transform.GetChild(0).name);
        string nameCraftObject=test.transform.GetChild(0).name;
        nameCraftObject = nameCraftObject.Replace("(Clone)", "").Trim();
        if (test.transform.childCount == 0)
        {
           // Debug.Log("buttonObject có đối tượng con.");
        }
        else
        {
            //Debug.Log("buttonObject không có đối tượng con.");

            DrawOverivederCT.Instance.setImageCt(test1);
           // DrawOverivederCT.Instance.AddToIventory("stoune");

            //Debug.Log(nameCraftObject);
           /* Recipe recipe = CraftSystem.Instance.FindRecipe(nameCraftObject);
            if(recipe != null)
            {
                foreach(KeyValuePair<string,int>ingredient in recipe.Ingredients)
                {
                    string ingredientName = ingredient.Key; // Tên nguyên liệu
                    int ingredientQuantity = ingredient.Value; // Số lượng nguyên liệu

                    Debug.Log($"{ingredientName}: {ingredientQuantity}");
                }
            }*/
           int count= CraftSystem.Instance.CountCompumentObject(nameCraftObject);
            DrawOverivederCT.Instance.CreateChileObjectCompument(count);
            DrawOverivederCT.Instance.printCompumentObject(nameCraftObject);
        }




    }

    private GameObject FindNextObject()
    {
        foreach(GameObject slot in CraftSystem.Instance.slotList)
        {

            // dieu kien de tim ra dung 
            if(slot.transform.childCount > 0 && slot.name == buttonObject.name)
            {
                return slot;
            }
        }
        return new GameObject();
    }

}
