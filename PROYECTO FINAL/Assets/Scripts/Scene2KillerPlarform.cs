using UnityEngine;

public class KillPlatformScene2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Registrar muerte
        if (GameManager.Instance != null)
            GameManager.Instance.AddDeath();

        // Solo funciona con el movimiento de caída libre
        FreeFallMovement fall = other.GetComponent<FreeFallMovement>();
        if (fall != null)
        {
            fall.Respawn();
        }
    }
}
