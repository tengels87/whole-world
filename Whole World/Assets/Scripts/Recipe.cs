using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe {
    public string recipeName = "unnamed recipe";
    public List<ItemUnit> ingredientList = new List<ItemUnit>();
    public ItemUnit outcome;
    [HideInInspector]
    public bool isUnlocked = true;

    public bool checkIngredientsFull(List<ItemUnit> inventoryList) {
        List<ItemUnit> ingre = new List<ItemUnit>(ingredientList);

        foreach (ItemUnit inventoryItem in inventoryList) {
            ItemUnit foundItem = ingre.Find(
                item => item.unitName.Equals(inventoryItem.unitName)
                );

            if (foundItem != null) {
                ingre.Remove(foundItem);
            }

            if (ingre.Count == 0) {
                return true;
            }
        }

        return (ingre.Count == 0);
    }

    public bool checkIngredientsTypeOnly(List<ItemUnit> inventoryList) {
        List<ItemUnit> ingre = new List<ItemUnit>(ingredientList);

        foreach (ItemUnit inventoryItem in inventoryList) {
            List<ItemUnit> foundItems = ingre.FindAll(
                item => item.unitName.Equals(inventoryItem.unitName)
                );

            foreach (ItemUnit item in foundItems) {
                if (item != null) {
                    ingre.Remove(item);
                }
            }

            if (ingre.Count == 0) {
                return true;
            }
        }

        return (ingre.Count == 0);
    }
}
