using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe 
{
    // Start is called before the first frame update
    public string ItemName;
    public Dictionary<string, int> Ingredients;
    public Recipe(string itemName, string recipeString)
    {
        ItemName=itemName;
        Ingredients= ParseRecipeString(recipeString);
    }

    private Dictionary<string,int> ParseRecipeString(string recipeString)
    {
        Dictionary<string,int> ingredients = new Dictionary<string,int>();
        string[] parts = recipeString.Split(',');
        foreach(string part in parts)
        {
            string[] ingredinentData = part.Trim().Split('x');
            int quantity = int.Parse(ingredinentData[0]);
            string name = ingredinentData[1];
            ingredients[name] = quantity;
        }
        return ingredients;
    }
}
