using UnityEngine;
using TMPro;

public class MessageUI : MonoBehaviour
{
    public static MessageUI instance;
    public TextMeshProUGUI messageText;
    public GameObject panel;
    public KeyCode dismissKey = KeyCode.Space;
    float hideTime;
    float shownTime;

    void Awake()
    {
        instance = this;
        SetMessage("");
    }

    void Update()
    {
        if (messageText.text == "") return;

        bool timeUp = hideTime > 0f && Time.time >= hideTime;
        bool dismissed = Time.time > shownTime + 0.2f && Input.GetKeyDown(dismissKey);

        if (timeUp || dismissed)
        {
            SetMessage("");
            hideTime = 0f;
        }
    }

    void SetMessage(string message)
    {
        messageText.text = message;
        if (panel != null) panel.SetActive(message != "");
    }

    public static void Show(string message, float duration = 3f)
    {
        if (instance == null) return;
        instance.SetMessage(message);
        instance.shownTime = Time.time;
        instance.hideTime = duration > 0f ? Time.time + duration : 0f;
    }
}