using UnityEngine;
using System.Collections;

public class IntroDialogue : MonoBehaviour
{
    [TextArea]
    public string[] lines =
    {
        "Damn, I can't believe my car decided to die on me on Halloween...",
        "Maybe I can find some help...",
        "Hmm... what does that sign say?",
        "Might as well bring my gun for safety."
    };

    public string controlsHint = "Press 1 to draw the gun. Hold Right-Click to aim, Left-Click to shoot.";
    public float hintDuration = 6f;

    PlayerShooting shooting;

    IEnumerator Start()
    {
        shooting = GetComponent<PlayerShooting>();
        if (shooting != null) shooting.canEquip = false;

        yield return null;

        foreach (string line in lines)
        {
            MessageUI.Show(line + "   [Space]", 0f);
            yield return null;

            while (MessageUI.instance != null && MessageUI.instance.messageText.text != "")
            {
                yield return null;
            }
        }

        if (shooting != null) shooting.canEquip = true;
        MessageUI.Show(controlsHint, hintDuration);
    }
}