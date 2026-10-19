using UnityEngine;
using UnityEngine.AI;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject finalWavePrefab;
    public Transform[] spawnPoints;
    public int[] waves = { 1, 2, 2, 3 };
    public float timeBetweenWaves = 2f;
    public float spawnSpread = 1.5f;
    public float navMeshSearchRadius = 5f;
    public GameObject rewardToReveal;
    public string startMessage = "Something's coming...";
    public string clearMessage = "That's all of them. Something shiny is over there...";

    int currentWave = -1;
    int aliveCount;
    bool started;
    bool finished;

    void Start()
    {
        if (rewardToReveal != null) rewardToReveal.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (started || !other.CompareTag("Player")) return;
        started = true;
        MessageUI.Show(startMessage);
        Invoke(nameof(NextWave), 1f);
    }

    void NextWave()
    {
        currentWave++;

        if (currentWave >= waves.Length)
        {
            finished = true;
            if (rewardToReveal != null) rewardToReveal.SetActive(true);
            MessageUI.Show(clearMessage);
            return;
        }

        bool isFinalWave = currentWave == waves.Length - 1;
        GameObject prefab = (isFinalWave && finalWavePrefab != null) ? finalWavePrefab : enemyPrefab;

        for (int i = 0; i < waves[currentWave]; i++)
        {
            Transform point = spawnPoints[i % spawnPoints.Length];
            Vector3 offset = new Vector3(Random.Range(-spawnSpread, spawnSpread), 0f, Random.Range(-spawnSpread, spawnSpread));
            Vector3 spawnPos = point.position + offset;

            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, navMeshSearchRadius, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }
            else
            {
                Debug.LogWarning("No NavMesh found near " + point.name + ". Check the bake and the hound's Agent Type.");
            }

            GameObject enemy = Instantiate(prefab, spawnPos, point.rotation);

            ZombieAI ai = enemy.GetComponentInChildren<ZombieAI>();
            if (ai != null) ai.detectionRange = 100f;

            Health health = enemy.GetComponentInChildren<Health>();
            if (health == null)
            {
                Debug.LogWarning(enemy.name + " has no Health component!");
                continue;
            }

            health.onDeath.AddListener(OnEnemyDied);
            aliveCount++;
        }
    }

    void OnEnemyDied()
    {
        aliveCount--;
        if (aliveCount <= 0 && !finished)
        {
            Invoke(nameof(NextWave), timeBetweenWaves);
        }
    }
}