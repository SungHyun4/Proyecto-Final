/// <summary>
/// Aplica una animación visual a objetos como coleccionables o plataformas decorativas.
/// Características:
/// - Rotación constante alrededor del eje Y, centrada en el propio objeto.
/// - Movimiento de flotación basado en una onda seno para dar efecto de suspensión.
/// 
/// Se recomienda usarlo en ítems, pickups o elementos visuales no interactivos.
/// </summary>
using UnityEngine;

public class FloatingRotator : MonoBehaviour
{
    [Header("Rotación")]
    public float rotationSpeed = 120f; // grados por segundo

    [Header("Flotación")]
    public float floatAmplitude = 0.25f;  // cuánto sube/baja
    public float floatFrequency = 2f;     // velocidad de flotación

    private Vector3 startPos;

    private void Start()
    {
        // guardamos la posición inicial local
        startPos = transform.localPosition;
    }

    private void Update()
    {
        // Rotación sobre su propio centro
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        // Flotación senoidal
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = new Vector3(
            startPos.x,
            newY,
            startPos.z
        );
    }
}
