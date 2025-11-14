using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target; // Erika

    [Header("Ajustes de cámara")]
    public float distance = 5f; // Distancia detrás del personaje
    public float height = 2f;   // Altura respecto al personaje
    public float followSpeed = 5f; // Suavizado del seguimiento

    [Header("Rotación")]
    public float rotationDamping = 8f; // Suavizado de rotación

    private void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada detrás del personaje
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        // Movimiento suave hacia esa posición
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Rotar suavemente para mirar al personaje
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationDamping * Time.deltaTime);
    }
}
