using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float rotationSpeed = 100f; // Velocidad de rotación
    public float gravity = -9.81f; // Gravedad aplicada al personaje
    public float jumpForce = 5f;   // Fuerza del salto
    public Camera mouseOrbitCamera; // Cámara orbital para referencia

    private CharacterController controller;
    private Vector3 velocity;

    // ANIMACIÓN
    private Animator anim;
    private static readonly int VelX = Animator.StringToHash("Velx");
    private static readonly int VelY = Animator.StringToHash("Vely");
    [SerializeField] private float animDamp = 0.05f;
    private float velXCur;
    private float velYCur;

    // checkpoint dinámico
    private Transform currentCheckpoint;

    // PlayerInput (para que no dé error)
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

        // detectar caída
        if (transform.position.y <= 0f)
        {
            // contar la caída en el GameManager
            if (GameManager.Instance != null)
                GameManager.Instance.AddFall();

            // volver al último checkpoint
            RespawnAtCheckpoint();
        }
    }

    private void MoveAndRotate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float vertical = 0f;
        float horizontal = 0f;

        // Movimiento adelante/atrás (W/S o flechas)
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            vertical = 1f;
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            vertical = -1f;

        // Rotación izquierda/derecha (A/D o flechas)
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            horizontal = -1f;
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            horizontal = 1f;

        Vector3 moveDirection = Vector3.zero;

        // Si la cámara orbital está activa
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

        // salto sin grounded (como lo pediste)
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ANIMACIÓN
        if (anim != null)
        {
            float targetVelX = 0f;

            if (mouseOrbitCamera != null && mouseOrbitCamera.gameObject.activeSelf)
            {
                targetVelX = horizontal;
            }

            float targetVelY = vertical;

            velXCur = Mathf.Lerp(velXCur, targetVelX, 1f - Mathf.Exp(-Time.deltaTime / animDamp));
            velYCur = Mathf.Lerp(velYCur, targetVelY, 1f - Mathf.Exp(-Time.deltaTime / animDamp));

            anim.SetFloat(VelX, velXCur);
            anim.SetFloat(VelY, velYCur);
        }
    }

    // llamado por los checkpoints
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
