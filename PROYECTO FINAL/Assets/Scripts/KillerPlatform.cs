using UnityEngine;

public class KillPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Sumar caída al GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.AddFall();

        // Reaparecer en el checkpoint
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.RespawnAtCheckpoint();
        }
    }
}