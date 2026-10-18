using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    
        Animator animator;
        public GameObject gunInHand;
        bool gunEquipped;

        PlayerInventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        animator=GetComponentInChildren<Animator>();

        inventory=GetComponent<PlayerInventory>();


        
        
    }

    // Update is called once per frame
    void Update()
    {



        if( gunEquipped=inventory.equippedWeapon=="WorldGun")
        {
            gunInHand.SetActive(gunEquipped);
            animator.SetBool("Aiming",Input.GetMouseButton(1));
 
        }

        



        


        
    }
}
