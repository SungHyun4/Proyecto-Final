using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem activateEffect;
    public AudioClip activateSound;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // siempre actualizar al último checkpoint
            player.SetCheckpoint(transform);

            // efectos solo la primera vez
            if (!activated)
            {
                activated = true;

                if (activateEffect != null)
                {
                    ParticleSystem ps = Instantiate(activateEffect, transform.position, Quaternion.identity);
                    ps.Play();
                    Destroy(ps.gameObject, 2f);
                }

                if (activateSound != null)
                    AudioSource.PlayClipAtPoint(activateSound, transform.position);
            }
        }
    }
}

