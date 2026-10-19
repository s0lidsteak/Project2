using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip footstepClip;
    public float volume = 0.5f;
    public float walkPitch = 1f;
    public float runPitch = 1.5f;

    CharacterController controller;
    AudioSource footstepSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footstepClip;
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
        footstepSource.spatialBlend = 0f;
        footstepSource.volume = volume;
    }

    void Update()
    {
        if (footstepClip == null) return;

        Vector3 flatVelocity = controller.velocity;
        flatVelocity.y = 0f;
        bool moving = controller.isGrounded && flatVelocity.magnitude > 0.2f;

        if (moving)
        {
            footstepSource.pitch = Input.GetKey(KeyCode.LeftShift) ? runPitch : walkPitch;
            if (!footstepSource.isPlaying) footstepSource.Play();
        }
        else if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }
}