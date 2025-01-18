using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawOverivederCT : MonoBehaviour
{
    // Start is called before the first frame update

    


    public static DrawOverivederCT Instance { get; set; }

    public GameObject imageCt;
    public GameObject TextC;
    public GameObject imageCompument;

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
    public void setImageCt(GameObject temp)
    {

        if (imageCt.transform.childCount != 0)
        {


            ClearChildren(imageCt);
        }
        
        
        GameObject newImageTest = Instantiate(temp);
        newImageTest.name = temp.name;
        newImageTest.transform.SetParent(imageCt.transform);
        newImageTest.transform.localPosition = Vector3.zero;
        newImageTest.transform.localRotation = Quaternion.identity;
        
    }

    void ClearChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void setImageCompument(GameObject temp)
    {

        if (imageCompument.transform.childCount != 0)
        {


            ClearChildren(imageCompument);
        }


        GameObject newImageTest = Instantiate(temp);
        newImageTest.transform.SetParent(imageCt.transform);
        newImageTest.transform.localPosition = Vector3.zero;
        newImageTest.transform.localRotation = Quaternion.identity;

    }
    public void AddToIventory(string name)
    {
        GameObject itemCompumentemp=Instantiate(Resources.Load<GameObject>(name),imageCompument.transform.position,imageCompument.transform.rotation);
        itemCompumentemp.transform.SetParent(imageCompument.transform);
    }


    public void AddToInventoryObjectCompument(GameObject temp1, string name , int count )
    {

        
       // Debug.Log(temp.transform.gameObject.name);
        
        GameObject prefab = Resources.Load<GameObject>(name);
        if (prefab == null)
        {
          //  Debug.LogError($"Prefab '{name}' not found in Resources!");
            return;
        }

        GameObject itemCompumentemp = Instantiate(prefab, temp1.transform.position, temp1.transform.rotation);
        string result = $"x{count}";
        CreateText(temp1, result, new Vector2(0, -15), 10, Color.white);
        if (itemCompumentemp != null)
        {
           // Debug.Log($"Đối tượng {itemCompumentemp.name} đã được tạo thành công!");
        }
        else
        {
            // Debug.LogError("Không thể tạo đối tượng, kiểm tra tên prefab hoặc đường dẫn.");
        }
        
        itemCompumentemp.transform.SetParent(temp1.transform);
        RectTransform rectTransform= itemCompumentemp.GetComponent<RectTransform>();
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
         //   Debug.LogError("itemToAdd không có RectTransform.");
        }
        // Debug.Log(name);
        //  Debug.Log(itemCompumentemp.transform.parent.name);
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



        Debug.Log($"Text object '{textObject.name}' created under '{parent.name}'.");
    }

    public void CreateChileObjectCompument(int count)
    {


        string spritePath = "GUI_Parts/Mini_frame0";
        Sprite loadedSprite = Resources.Load<Sprite>(spritePath);

        if (imageCompument.transform.childCount != 0)
        {
            ClearChildren(imageCompument);
        }
        GameObject newGameObject = new GameObject();
        for (int i=0;i < count; i++)
        {
            GameObject child = Instantiate(newGameObject, imageCompument.transform);
            child.name = $"child_{i + 1}";
            
            child.transform.SetParent(imageCompument.transform);
            child.transform.SetSiblingIndex(i);
            RectTransform rect = child.AddComponent<RectTransform>();
            child.AddComponent<CanvasRenderer>();
            Image image = child.AddComponent<Image>();
            image.sprite = loadedSprite;
            int startPos = (count-1) * -25;
            if(rect != null)
            {
                rect.localPosition = new Vector3(startPos+(i)*50,0,0);
                rect.sizeDelta=new Vector2 (50,50);
            }
        }

        Destroy(newGameObject);
    }

    public GameObject searchChildObjectCompument(int i)
    {
        foreach(Transform child in imageCompument.transform)
        {
            if (child.gameObject.name == $"child_{i+1}") {
            
                return child.gameObject;
            }
        }

        return new GameObject();
    }


    public void DeleteAllChildObject(Transform parrent)
    {
        foreach(Transform child in parrent)
        {
            Destroy(child.gameObject);
        }
    }


    public void ClearnFull()
    {
        DeleteAllChildObject(imageCt.transform);
        DeleteAllChildObject(imageCompument.transform);
    }
    public void printCompumentObject(string nameCt)
    {
        Recipe recipe = CraftSystem.Instance.FindRecipe(nameCt);
        int count = 0;
        if (recipe != null)
        {
            foreach (KeyValuePair<string, int> ingredient in recipe.Ingredients)
            {
                string ingredientName = ingredient.Key; // Tên nguyên liệu
                int ingredientQuantity = ingredient.Value; // Số lượng nguyên liệu
                                                           // print 
                Debug.Log(ingredientName);
                Debug.Log(ingredientQuantity);
                if (DrawOverivederCT.Instance.imageCompument.transform.childCount != 0)
                {
                    GameObject temp = imageCompument.transform.GetChild(count).gameObject;
                    Debug.Log(temp.transform.parent.name+ " "+count+ " "+ temp.name);
                    DrawOverivederCT.Instance.AddToInventoryObjectCompument(temp, ingredientName, ingredientQuantity);
                }
                
                count++;
                //DrawOverivederCT.Instance.setImageCompument()



            }

            //  Debug.Log(count);
        }
    }

    public string returnNameCt()
    {
       return imageCt.transform.GetChild(0).gameObject.name;
    }

}
