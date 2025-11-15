using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Tooltip("Puntos que suma o resta. Positivo = suma, negativo = resta.")]
    public int scoreValue = 10;

    [Tooltip("Cuenta como elemento recogido para el total.")]
    public bool countAsCollected = true;

    [Header("Feedback visual y sonoro")]
    public AudioClip pickupSound;
    public ParticleSystem pickupParticles;

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

        if (pickupParticles != null)
        {
            ParticleSystem ps = Instantiate(pickupParticles, transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, 2f);
        }

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }
}
