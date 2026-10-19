using UnityEngine;

public class SignRead : MonoBehaviour
{
    [TextArea]
    public string message = "TRAIL — Cabin: turn LEFT at the end of the trail. Lost campers are welcome to get supplies.";

    bool playerNearby;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            MessageUI.Show(message + "   [Space]", 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = true;
        MessageUI.Show("Press E to read", 0f);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = false;
        MessageUI.Show("", 0f);
    }
}