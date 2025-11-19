/// <summary>
/// Plataforma letal para la primera escena.
/// Cuando el jugador entra en su trigger:
/// - Suma una muerte al GameManager.
/// - Reaparece al jugador en el último checkpoint registrado.
/// Solo afecta a objetos con el tag "Player".
/// </summary>
using UnityEngine;

public class KillPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Sumar muerte al GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.AddDeath();

        // Reaparecer en el último checkpoint del jugador
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.RespawnAtCheckpoint();
        }
    }
}
