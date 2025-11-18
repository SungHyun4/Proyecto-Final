using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float boostAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        FreeFallMovement fall = other.GetComponent<FreeFallMovement>();
        if (fall != null)
        {
            fall.IncreaseSpeedPermanently(boostAmount);
        }

        // destruir la gema
        Destroy(gameObject);
    }
}
