using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void AddItem(string item)
    {
        items.Add(item);
        Debug.Log("Picked up: " + item + " | Inventory: " + string.Join(", ", items));
    }

    public bool HasItem(string item)
    {
        return items.Contains(item);
    }
}