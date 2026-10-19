using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class ZombieAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Behavior")]
    public float detectionRange = 12f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;
    public int attackDamage = 10;
    public float destroyDelay = 5f;

    [Header("Pause")]
    public float howlDuration = 1.5f;
    public float minTimeBetweenHowls = 4f;
    public float maxTimeBetweenHowls = 7f;
    public float minHowlDistance = 4f;

    [Header("Charger")]
    public bool isCharger = false;
    public float chargeSpeed = 7f;
    public float recoverTime = 2f;

    NavMeshAgent agent;
    Animator animator;
    Health health;
    Health playerHealth;
    float lastAttackTime = -999f;
    float howlEndTime;
    float nextHowlTime;
    float recoverEndTime;
    bool hasSpotted;
    bool charging;
    bool isDead;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        if (isCharger) agent.speed = chargeSpeed;

        health.onDeath.AddListener(Die);
        health.onDamaged.AddListener(() => OnHit());
    }

    void Update()
    {
        if (isDead || player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!hasSpotted && distance <= detectionRange)
        {
            hasSpotted = true;
            Howl();
        }
        else if (!isCharger && hasSpotted && Time.time >= nextHowlTime && distance > minHowlDistance)
        {
            Howl();
        }

        if (Time.time < howlEndTime)
        {
            agent.isStopped = true;
            FacePlayer();
        }
        else if (isCharger && hasSpotted)
        {
            UpdateCharger(distance);
        }
        else if (distance <= attackRange)
        {
            agent.isStopped = true;
            FacePlayer();
            TryAttack();
        }
        else if (hasSpotted)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }

        if (animator != null) animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void UpdateCharger(float distance)
    {
        if (charging)
        {
            if (distance <= attackRange)
            {
                TryAttack();
                EndCharge();
            }
            else if (!agent.pathPending && agent.remainingDistance <= 0.5f)
            {
                EndCharge();
            }
        }
        else if (Time.time >= recoverEndTime)
        {
            StartCharge();
        }
        else
        {
            agent.isStopped = true;
            FacePlayer();
        }
    }

    void StartCharge()
    {
        charging = true;
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void EndCharge()
    {
        charging = false;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        recoverEndTime = Time.time + recoverTime;
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        if (animator != null) animator.SetTrigger("Attack");
        if (playerHealth != null) playerHealth.TakeDamage(attackDamage);
    }

    void Howl()
    {
        howlEndTime = Time.time + howlDuration;
        nextHowlTime = howlEndTime + Random.Range(minTimeBetweenHowls, maxTimeBetweenHowls);
    }

    void OnHit()
    {
        if (isDead) return;
        if (!hasSpotted)
        {
            hasSpotted = true;
            nextHowlTime = Time.time + Random.Range(minTimeBetweenHowls, maxTimeBetweenHowls);
        }
        if (animator != null) animator.SetTrigger("Hit");
    }

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        if (agent.isOnNavMesh) agent.isStopped = true;
        agent.enabled = false;

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }

        if (animator != null) animator.SetTrigger("Die");
        Destroy(gameObject, destroyDelay);
    }
}