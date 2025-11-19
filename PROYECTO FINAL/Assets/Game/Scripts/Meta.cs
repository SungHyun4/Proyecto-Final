/// <summary>
/// Punto final del nivel. 
/// Cuando el jugador entra en el trigger:
/// - Activa el panel final a través del FinalPanelController.
/// - Detiene el movimiento del jugador, ya sea en modo caída libre o caminando.
/// Solo reacciona a objetos con el tag "Player".
/// </summary>
using UnityEngine;

public class GoalFinish : MonoBehaviour
{
    public FinalPanelController finalPanelController; // el controller, NO el panel

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Mostrar el panel final correctamente
        finalPanelController.ShowFinalPanel();

        // Congelar movimiento del jugador
        FreeFallMovement fall = other.GetComponent<FreeFallMovement>();
        if (fall != null)
            fall.enabled = false;

        PlayerMovement walk = other.GetComponent<PlayerMovement>();
        if (walk != null)
            walk.enabled = false;
    }
}
