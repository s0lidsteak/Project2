using UnityEngine;

public class CabinDoor : MonoBehaviour
{
    public string requiredItem = "Key1";
    public string rewardItem = "GasCan";
    public float interactRange = 3f;

    Transform player;
    PlayerInventory inventory;
    bool showingPrompt;
    bool opened;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null)
        {
            Debug.LogWarning("CabinDoor: nothing is tagged Player");
            return;
        }
        player = p.transform;
        inventory = p.GetComponentInChildren<PlayerInventory>();
        if (inventory == null) inventory = p.GetComponentInParent<PlayerInventory>();
    }

    void Update()
    {
        if (opened || player == null) return;

        bool near = Vector3.Distance(transform.position, player.position) <= interactRange;

        if (near && !showingPrompt)
        {
            MessageUI.Show("Press E to open", 0f);
            showingPrompt = true;
        }
        else if (!near && showingPrompt)
        {
            MessageUI.Show("", 0f);
            showingPrompt = false;
        }

        if (!near || !Input.GetKeyDown(KeyCode.E)) return;

        if (inventory == null || !inventory.HasItem(requiredItem))
        {
            MessageUI.Show("It's locked. I need a key.");
            return;
        }

        opened = true;
        foreach (Renderer r in GetComponentsInChildren<Renderer>()) r.enabled = false;
        foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = false;

        inventory.AddItem(rewardItem);
        MessageUI.Show("Found a gas can inside! Back to the car.", 4f);
    }
}