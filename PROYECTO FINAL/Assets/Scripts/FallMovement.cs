using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FreeFallMovement : MonoBehaviour
{
    public float horizontalSpeed = 8f;   // Velocidad lateral (A/D)
    public float verticalSpeed = 8f;     // Velocidad frontal (W/S)
    public float gravity = -20f;         // Gravedad realista
    private Vector3 velocity;

    private CharacterController controller;
    private Animator anim;
    private static readonly int VelX = Animator.StringToHash("Velx");
    private static readonly int VelY = Animator.StringToHash("Vely");

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveFreeFall();

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Muerte si cae muy bajo
        if (transform.position.y <= -80f)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddDeath();

            Respawn();
        }
    }

    private void MoveFreeFall()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float moveX = 0f; // A / D
        float moveZ = 0f; // W / S

        // Movimiento lateral (A/D)
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            moveX = -1f;
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            moveX = 1f;

        // Movimiento adelante/atrás (W/S)
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            moveZ = 1f;
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            moveZ = -1f;

        // Movimiento global (NO depende de la rotación del personaje)
        Vector3 move =
            (Vector3.forward * (moveZ * verticalSpeed)) +
            (Vector3.right * (moveX * horizontalSpeed));

        controller.Move(move * Time.deltaTime);

        // Animaciones
        if (anim != null)
        {
            anim.SetFloat(VelX, moveX);
            anim.SetFloat(VelY, moveZ);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Cualquier colisión mata (ya no usa tags)
        if (hit.collider != null)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddDeath();

            Respawn();
        }
    }

    // CORREGIDO: ahora es PUBLIC
    public void Respawn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    //  Boost permanente
    public void IncreaseSpeedPermanently(float amount)
    {
        horizontalSpeed += amount;
        verticalSpeed += amount;
    }
}
