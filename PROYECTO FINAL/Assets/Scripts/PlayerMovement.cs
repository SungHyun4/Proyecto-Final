/// <summary>
/// Controla todo el movimiento del jugador en la Escena 1.
/// Incluye:
/// - Movimiento adelante/atrás (W/S) y rotación (A/D).
/// - Salto con verificación real de suelo mediante un groundCheck.
/// - Aplicación de gravedad manual.
/// - Integración con Animator para actualizar parámetros de movimiento y salto.
/// - Sistema de checkpoints: guarda la posición y permite respawn.
/// - Detecta cuando el jugador cae fuera del nivel y registra una muerte en el GameManager.
/// 
/// Requiere:
/// - CharacterController en el mismo GameObject.
/// - Animator con parámetros "Velx", "Vely" e "IsGrounded".
/// - Un objeto groundCheck con layerMask configurado.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float gravity = -9.81f;
    public float jumpForce = 5f;
    public Camera mouseOrbitCamera;

    private CharacterController controller;
    private Vector3 velocity;

    // --- GROUNDED CHECK ---
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded;

    // ANIMACIÓN
    private Animator anim;
    private static readonly int VelX = Animator.StringToHash("Velx");
    private static readonly int VelY = Animator.StringToHash("Vely");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    [SerializeField] private float animDamp = 0.05f;
    private float velXCur;
    private float velYCur;

    // checkpoint dinámico
    private Transform currentCheckpoint;

    // PlayerInput
    private Vector2 moveInput;
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // crear un checkpoint inicial invisible
        GameObject initialCheckpoint = new GameObject("InitialCheckpoint");
        initialCheckpoint.transform.position = transform.position;
        currentCheckpoint = initialCheckpoint.transform;
    }

    private void Update()
    {
        MoveAndRotate();

        // detectar muerte por caer al vacío
        if (transform.position.y <= 0f)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddDeath();

            RespawnAtCheckpoint();
        }
    }

    private void MoveAndRotate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float vertical = 0f;
        float horizontal = 0f;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            vertical = 1f;
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            vertical = -1f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            horizontal = -1f;
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            horizontal = 1f;

        Vector3 moveDirection = Vector3.zero;

        if (mouseOrbitCamera != null && mouseOrbitCamera.gameObject.activeSelf)
        {
            Vector3 forward = mouseOrbitCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();
            moveDirection = forward * vertical;
        }
        else
        {
            moveDirection = transform.forward * vertical;

            if (horizontal != 0)
            {
                float rotation = horizontal * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, rotation, 0);
            }
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // GROUNDED CHECK REAL
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        }
        else
        {
            isGrounded = controller.isGrounded;
        }

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ANIMACIONES
        if (anim != null)
        {
            float targetVelX = (mouseOrbitCamera != null && mouseOrbitCamera.gameObject.activeSelf) ? horizontal : 0f;
            float targetVelY = vertical;

            velXCur = Mathf.Lerp(velXCur, targetVelX, 1f - Mathf.Exp(-Time.deltaTime / animDamp));
            velYCur = Mathf.Lerp(velYCur, targetVelY, 1f - Mathf.Exp(-Time.deltaTime / animDamp));

            anim.SetFloat(VelX, velXCur);
            anim.SetFloat(VelY, velYCur);
            anim.SetBool(IsGroundedHash, isGrounded);
        }
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void RespawnAtCheckpoint()
    {
        if (currentCheckpoint == null) return;

        controller.enabled = false;
        transform.position = currentCheckpoint.position;
        controller.enabled = true;

        velocity = Vector3.zero;
    }
}
