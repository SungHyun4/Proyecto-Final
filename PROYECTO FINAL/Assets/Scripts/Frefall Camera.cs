using UnityEngine;

public class FreeFallCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, -6f);
    public float followSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // Seguir al jugador en posición
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);

        // Mirar SIEMPRE hacia -Y global
        transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
    }
}
