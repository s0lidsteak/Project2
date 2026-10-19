using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour
{
    public static WinScreen instance;
    public GameObject winPanel;
    public float quitDelay = 5f;

    void Awake()
    {
        instance = this;
        if (winPanel != null) winPanel.SetActive(false);
    }

    public static void Show()
    {
        if (instance != null) instance.StartCoroutine(instance.WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSecondsRealtime(quitDelay);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}