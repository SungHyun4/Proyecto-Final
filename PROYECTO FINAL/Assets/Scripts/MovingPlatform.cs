using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Distancias de movimiento")]
    [Tooltip("Distancia máxima que se moverá verticalmente (Y). 0 = sin movimiento vertical.")]
    public float verticalDistance = 3f;

    [Tooltip("Distancia máxima que se moverá horizontalmente (X). 0 = sin movimiento horizontal.")]
    public float horizontalDistance = 0f;

    [Header("Velocidades de movimiento")]
    [Tooltip("Velocidad vertical (arriba y abajo). 0 = sin movimiento vertical.")]
    public float verticalSpeed = 2f;

    [Tooltip("Velocidad horizontal (izquierda y derecha). 0 = sin movimiento horizontal.")]
    public float horizontalSpeed = 0f;

    [Header("Tiempo de espera en extremos")]
    public float waitTime = 0.5f;

    private Vector3 startPos;
    private bool goingUp = true;
    private bool goingRight = true;
    private float waitCounter = 0f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (waitCounter > 0f)
        {
            waitCounter -= Time.deltaTime;
            return;
        }

        Vector3 pos = transform.position;

        // --- Movimiento vertical ---
        if (verticalDistance != 0f && verticalSpeed != 0f)
        {
            float targetY = startPos.y + (goingUp ? verticalDistance : 0);
            pos.y = Mathf.MoveTowards(pos.y, targetY, verticalSpeed * Time.deltaTime);

            if (Mathf.Abs(pos.y - targetY) < 0.01f)
            {
                goingUp = !goingUp;
                waitCounter = waitTime;
            }
        }

        // --- Movimiento horizontal ---
        if (horizontalDistance != 0f && horizontalSpeed != 0f)
        {
            float targetX = startPos.x + (goingRight ? horizontalDistance : 0);
            pos.x = Mathf.MoveTowards(pos.x, targetX, horizontalSpeed * Time.deltaTime);

            if (Mathf.Abs(pos.x - targetX) < 0.01f)
            {
                goingRight = !goingRight;
                waitCounter = waitTime;
            }
        }

        transform.position = pos;
    }

    // Para que el jugador viaje con la plataforma
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(null);
    }
}
