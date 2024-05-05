using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 3f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float mass = 1f;
    [SerializeField] float acceleration = 20f;

    [SerializeField] float baseStepSpeed = 0.5f;
    [SerializeField] AudioSource footstepAudioSource;
    [SerializeField] AudioClip[] footstepAudioClips;
    private float footstepTimer = 0;
    internal float stepSpeed;

    public Transform cameraTransform;

    public bool IsGrounded => controller.isGrounded;

    public float Height
    {
        get => controller.height;
        set => controller.height = value;
    }

    public event Action OnBeforeMove;
    public event Action<bool> OnGroundStateChange;

    internal float movementSpeedMultiplier;

    CharacterController controller;
    Health health;
    internal Vector3 velocity;
    Vector2 look;

    bool wasGrounded;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        health = GetComponent<Health>();
        moveAction = playerInput.actions["move"];
        lookAction = playerInput.actions["look"];
    }

    private void OnDeath()
    {
        GameObject.FindGameObjectWithTag("Restart").GetComponent<RestartMenu>().ShowScreen();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        UpdateGround();
        UpdateGravity();
        UpdateMovement();
        UpdateLook();

        if(health.health < 0)
        {
            OnDeath();
        }
    }

    void UpdateGround()
    {
        if (wasGrounded != IsGrounded)
        {
            OnGroundStateChange?.Invoke(IsGrounded);
            wasGrounded = IsGrounded;
        }
    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }

    Vector3 GetMovementInput()
    {
        if (Interactable.IsInteracting) return Vector3.zero;
        var moveInput = moveAction.ReadValue<Vector2>();
        var input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        input *= movementSpeed * movementSpeedMultiplier;
        return input;
    }

    void UpdateMovement()
    {
        movementSpeedMultiplier = 1f;
        stepSpeed = baseStepSpeed;
        OnBeforeMove?.Invoke();

        var input = GetMovementInput();

        var factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        controller.Move(velocity * Time.deltaTime);
        HandleFootsteps(stepSpeed);
    }

    void UpdateLook()
    {
        var lookInput = lookAction.ReadValue<Vector2>();
        look.x += lookInput.x * mouseSensitivity;
        look.y += lookInput.y * mouseSensitivity;

        look.y = Mathf.Clamp(look.y, -89f, 89f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void HandleFootsteps(float stepSpeed)
    {
        if (!controller.isGrounded) return;
        if (controller.velocity == Vector3.zero) return;
        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            PlayStepSound();
            footstepTimer = stepSpeed;
        }
    }

    public void PlayStepSound()
    {
        footstepAudioSource.PlayOneShot(footstepAudioClips[UnityEngine.Random.Range(0, footstepAudioClips.Length - 1)]);
    }
}
