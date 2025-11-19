/// <summary>
/// Plataforma mortal exclusiva para la Escena 2 (caída libre).
/// - Detecta cuando el jugador entra en el trigger.
/// - Registra una muerte en el GameManager.
/// - Llama al método Respawn() del FreeFallMovement para reiniciar la escena.
/// 
/// Solo afecta a objetos con el tag "Player".
/// </summary>
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
