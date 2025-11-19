/// <summary>
/// Controla la lógica de un objeto coleccionable. 
/// Suma o resta puntaje al jugador y se destruye al ser recogido.
/// </summary>
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Tooltip("Puntos que suma o resta. Positivo = suma, negativo = resta.")]
    public int scoreValue = 10;

    [Tooltip("Cuenta como elemento recogido para el total.")]
    public bool countAsCollected = true;

    [Header("Feedback visual y sonoro")]
    public AudioClip pickupSound;
    // ParticleSystem pickupParticles;  <-- eliminado por solicitud

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player")) return;

        collected = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            if (countAsCollected)
                GameManager.Instance.AddCollected();
        }

        // se activa un audio al colisionar 
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}
