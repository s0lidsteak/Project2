using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float runSpeed = 4f;
    public float turnSpeed = 10f;
    public float gravity = -9.8f;

    [Header("Debug")]
    public bool logGrounded = false;

    CharacterController controller;
    Animator animator;
    PlayerShooting shooting;
    Transform cam;
    float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        shooting = GetComponent<PlayerShooting>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 direction = camForward * z + camRight * x;
        direction = Vector3.ClampMagnitude(direction, 1f);

        if (shooting != null && shooting.isAiming)
        {
            direction = Vector3.zero;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && direction.sqrMagnitude > 0.01f;
        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = direction * currentSpeed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        if (animator != null)
        {
            float animSpeed = direction.magnitude * (isRunning ? 2f : 1f);
            animator.SetFloat("Speed", animSpeed);
        }

        if (logGrounded)
        {
            Debug.Log("Grounded: " + controller.isGrounded);
        }
    }
}