using UnityEngine;
using Unity.Cinemachine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject gunInHand;
    public GameObject aimCam;
    public GameObject crosshair;
    public Transform cameraTarget;
    public float aimTurnSpeed = 1.75f;
    public bool canEquip = true;
    public bool isAiming;

    [Header("Follow Cam")]
    public CinemachineOrbitalFollow followOrbit;
    public CinemachineInputAxisController followInput;

    [Header("Shooting")]
    public float range = 50f;
    public int damage = 25;
    public float fireCooldown = 0.3f;
    public LayerMask hitLayers = ~0;

    [Header("Audio")]
    public AudioClip gunshotSound;

    Animator animator;
    AudioSource audioSource;
    bool gunEquipped;
    bool wasAiming;
    float pitch;
    float nextFireTime;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canEquip && Input.GetKeyDown(KeyCode.Alpha1)) gunEquipped = !gunEquipped;
        gunInHand.SetActive(gunEquipped);

        isAiming = gunEquipped && Input.GetMouseButton(1);
        animator.SetBool("Aiming", isAiming);
        if (aimCam != null) aimCam.SetActive(isAiming);
        if (crosshair != null) crosshair.SetActive(isAiming);
        if (followInput != null) followInput.enabled = !isAiming;

        if (isAiming && !wasAiming)
        {
            Vector3 look = Camera.main.transform.forward;
            look.y = 0f;
            if (look.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(look);
            pitch = 0f;
        }

        if (isAiming)
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X") * aimTurnSpeed, 0f);
            pitch -= Input.GetAxis("Mouse Y") * aimTurnSpeed;
            pitch = Mathf.Clamp(pitch, -30f, 40f);
            if (cameraTarget != null) cameraTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireCooldown;
                animator.SetTrigger("Shoot");
                Fire();
            }
        }
        else if (wasAiming)
        {
            if (cameraTarget != null) cameraTarget.localRotation = Quaternion.identity;
            if (followOrbit != null)
            {
                followOrbit.HorizontalAxis.Value = transform.eulerAngles.y;
                followOrbit.VerticalAxis.Value = followOrbit.VerticalAxis.Center;
            }
        }

        wasAiming = isAiming;
    }

    void Fire()
    {
        if (audioSource != null && gunshotSound != null) audioSource.PlayOneShot(gunshotSound);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Hit: " + hit.collider.name);
            Health targetHealth = hit.collider.GetComponentInParent<Health>();
            if (targetHealth != null) targetHealth.TakeDamage(damage);
        }
    }
}