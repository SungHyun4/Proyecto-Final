using UnityEngine;

public class GoalFinish : MonoBehaviour
{
    public GameObject finalPanel; // asignas la UI en el inspector

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Activar pantalla final
        finalPanel.SetActive(true);

        // congelar movimiento del jugador
        FreeFallMovement fall = other.GetComponent<FreeFallMovement>();
        if (fall != null)
            fall.enabled = false;
    }
}
