using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    public float restartDelay = 3f;
    public AudioClip hurtSound;
    public Image damageFlash;
    public float flashStrength = 0.4f;
    public float flashFadeSpeed = 2f;

    Health health;
    Animator animator;
    AudioSource audioSource;
    bool isDead;

    void Start()
    {
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        health.onDamaged.AddListener(Hurt);
        health.onDeath.AddListener(Die);
    }

    void Update()
    {
        if (damageFlash != null && damageFlash.color.a > 0f)
        {
            Color c = damageFlash.color;
            c.a = Mathf.MoveTowards(c.a, 0f, flashFadeSpeed * Time.deltaTime);
            damageFlash.color = c;
        }
    }

    void Hurt()
    {
        if (isDead) return;

        if (damageFlash != null)
        {
            Color c = damageFlash.color;
            c.a = flashStrength;
            damageFlash.color = c;
        }

        if (audioSource != null && hurtSound != null) audioSource.PlayOneShot(hurtSound);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        PlayerMovement m = GetComponent<PlayerMovement>();
        if (m != null) m.enabled = false;
        PlayerShooting s = GetComponent<PlayerShooting>();
        if (s != null) s.enabled = false;

        if (animator != null)
        {
            animator.SetBool("Aiming", false);
            animator.SetTrigger("Die");
        }

        Invoke(nameof(Restart), restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}