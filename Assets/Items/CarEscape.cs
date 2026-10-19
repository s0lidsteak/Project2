using UnityEngine;

public class CarEscape : MonoBehaviour
{
    bool escaped;

    void OnTriggerEnter(Collider other)
    {
        if (escaped || !other.CompareTag("Player")) return;

        PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
        bool hasGas = inventory != null && inventory.HasItem("GasCan");

        if (!hasGas)
        {
            MessageUI.Show("Out of gas. I need to find some.");
            return;
        }

        if (!WaveGroups.allCleared)
        {
            MessageUI.Show("Those hounds will tear me apart before I get it started. I need to deal with them first.");
            return;
        }

        escaped = true;
        other.GetComponent<PlayerMovement>().enabled = false;
        other.GetComponent<PlayerShooting>().enabled = false;
        MessageUI.Show("", 0f);
        WinScreen.Show();
    }
}