using UnityEngine;

public class PickUpScript : MonoBehaviour
{

    public string item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    
    {

        if (other.CompareTag("Player"))
        {

           PlayerInventory inventory = other.GetComponent<PlayerInventory>(); 

           inventory.AddItem(item);
        }
        



        


    }
}
