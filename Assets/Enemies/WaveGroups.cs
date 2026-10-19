using UnityEngine;

public class WaveGroups : MonoBehaviour
{
    public static bool allCleared;

    public GameObject[] waves;
    public float timeBetweenWaves = 4f;
    public GameObject rewardToReveal;
    public AudioClip howlSound;
    public float howlLeadTime = 2f;
    public string startMessage = "Something's coming...";
    public string waveMessage = "More of them...";
    public string clearMessage = "That's all of them. Time to get out of here.";

    AudioSource audioSource;
    int currentWave = -1;
    int aliveCount;
    bool started;
    bool finished;

    void Start()
    {
        allCleared = false;
        audioSource = GetComponent<AudioSource>();
        foreach (GameObject w in waves)
        {
            if (w != null) w.SetActive(false);
        }
        if (rewardToReveal != null) rewardToReveal.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (started || !other.CompareTag("Player")) return;
        started = true;
        MessageUI.Show(startMessage);
        Invoke(nameof(Howl), 0f);
        Invoke(nameof(NextWave), howlLeadTime);
    }

    void Howl()
    {
        if (audioSource != null && howlSound != null) audioSource.PlayOneShot(howlSound);
    }

    void NextWave()
    {
        currentWave++;

        if (currentWave >= waves.Length)
        {
            finished = true;
            allCleared = true;
            if (rewardToReveal != null) rewardToReveal.SetActive(true);
            MessageUI.Show(clearMessage);
            return;
        }

        GameObject wave = waves[currentWave];
        wave.SetActive(true);
        if (currentWave > 0) MessageUI.Show(waveMessage, 2f);

        aliveCount = 0;
        foreach (Health h in wave.GetComponentsInChildren<Health>())
        {
            h.onDeath.AddListener(OnEnemyDied);
            ZombieAI ai = h.GetComponent<ZombieAI>();
            if (ai != null) ai.detectionRange = 100f;
            aliveCount++;
        }

        if (aliveCount == 0) Invoke(nameof(NextWave), timeBetweenWaves);
    }

    void OnEnemyDied()
    {
        aliveCount--;
        if (aliveCount <= 0 && !finished)
        {
            if (currentWave + 1 < waves.Length) Invoke(nameof(Howl), timeBetweenWaves - howlLeadTime);
            Invoke(nameof(NextWave), timeBetweenWaves);
        }
    }
}