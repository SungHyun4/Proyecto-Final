/// <summary>
/// Aumenta permanentemente la velocidad de caída del jugador en la Escena 2.
/// - Detecta cuando el jugador entra en el trigger.
/// - Reduce el valor de gravedad del FreeFallMovement (haciendo la caída más rápida).
/// - Se destruye inmediatamente después de activarse.
/// 
/// Solo funciona con objetos que tengan el tag "Player".
/// </summary>
using UnityEngine;

public class GravityBoost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        FreeFallMovement fall = other.GetComponent<FreeFallMovement>();
        if (fall != null)
        {
            fall.gravity -= 3f;   // Suma -3 al valor actual de gravedad
            Debug.Log("Nueva gravedad: " + fall.gravity);
        }

        Destroy(gameObject); // elimina el booster tras activarse
    }
}
