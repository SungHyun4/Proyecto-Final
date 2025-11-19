/// <summary>
/// Marca un punto de control (checkpoint) que actualiza la posición de reaparición
/// del jugador cuando entra en contacto con él. Solo se activa una vez.
/// </summary>
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public AudioClip activateSound;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Actualizar siempre el último checkpoint alcanzado
            player.SetCheckpoint(transform);

            // Sonido solo la primera vez
            if (!activated)
            {
                activated = true;

                if (activateSound != null)
                    AudioSource.PlayClipAtPoint(activateSound, transform.position);
            }
        }
    }
}
