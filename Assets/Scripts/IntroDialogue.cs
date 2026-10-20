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

    PlayerShooting shooting;
    float mouseTotal;

    IEnumerator Start()
    {
        shooting = GetComponent<PlayerShooting>();
        if (shooting != null) shooting.canEquip = false;

        yield return null;

        foreach (string line in lines)
        {
            MessageUI.Show(line + "   [Space]", 0f);
            yield return null;
            while (MessageUI.instance != null && MessageUI.instance.messageText.text != "") yield return null;
        }

        yield return Step("Use W A S D to walk.", () => IsMoving(), 1f);
        yield return Step("Hold Shift while moving to run.", () => IsMoving() && Input.GetKey(KeyCode.LeftShift), 0.75f);

        mouseTotal = 0f;
        yield return Step("Move the mouse to look around.", () => MouseMoved(20f), 0f);

        if (shooting != null) shooting.canEquip = true;
        yield return Step("Press 1 to draw your gun.", () => shooting != null && shooting.gunInHand != null && shooting.gunInHand.activeSelf, 0f);
        yield return Step("Hold Right Mouse to aim.", () => shooting != null && shooting.isAiming, 0.5f);

        mouseTotal = 0f;
        yield return Step("Keep holding and move the mouse to aim around.", () => shooting != null && shooting.isAiming && MouseMoved(15f), 0f);
        yield return Step("Left Click to shoot.", () => shooting != null && shooting.isAiming && Input.GetMouseButtonDown(0), 0f);

        MessageUI.Show("Press E to read signs and pick things up. Press Space to close messages.   [Space]", 0f);
    }

    IEnumerator Step(string message, System.Func<bool> done, float holdTime)
    {
        MessageUI.Show(message, 0f);
        float held = 0f;

        while (true)
        {
            if (MessageUI.instance != null && MessageUI.instance.messageText.text == "")
            {
                MessageUI.Show(message, 0f);
            }

            if (done())
            {
                held += Time.deltaTime;
                if (held >= holdTime) break;
            }

            yield return null;
        }

        MessageUI.Show("Nice.", 1f);
        yield return new WaitForSeconds(0.8f);
    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f;
    }

    bool MouseMoved(float amount)
    {
        mouseTotal += Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
        return mouseTotal >= amount;
    }
}