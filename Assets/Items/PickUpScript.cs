using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public string item = "Key1";
    public string pickupMessage = "Picked up the key";
    public float pickupRange = 2f;

    Transform player;
    PlayerInventory inventory;
    bool showingPrompt;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
        {
            Debug.LogWarning("PickUpScript: nothing is tagged Player");
            return;
        }

        player = p.transform;
        inventory = p.GetComponentInChildren<PlayerInventory>();
        if (inventory == null) inventory = p.GetComponentInParent<PlayerInventory>();
    }

    void Update()
    {
        if (player == null) return;

        bool near = Vector3.Distance(transform.position, player.position) <= pickupRange;

        if (near && !showingPrompt)
        {
            MessageUI.Show("Press E to pick up", 0f);
            showingPrompt = true;
        }
        else if (!near && showingPrompt)
        {
            MessageUI.Show("", 0f);
            showingPrompt = false;
        }

        if (near && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null) inventory.AddItem(item);
            MessageUI.Show(pickupMessage);
            Destroy(gameObject);
        }
    }
}