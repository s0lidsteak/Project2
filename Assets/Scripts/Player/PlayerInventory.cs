using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{

    public List<string>items;
    public string equippedWeapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) { Equip("Bat"); }       
    if (Input.GetKeyDown(KeyCode.Alpha2)) { Equip("WorldGun"); }  
    if (Input.GetKeyDown(KeyCode.Alpha3)) { Equip("Bottle"); }    

        
        
    }

    public void AddItem(string item)
    
    {
        items.Add(item);
    }

    public bool HasItem(string item)
    {
        return items.Contains(item);
        
    }

    void Equip(string name)
    {
        if (HasItem(name))
        {

            
        }
        equippedWeapon=name;
        
    }

    
}
