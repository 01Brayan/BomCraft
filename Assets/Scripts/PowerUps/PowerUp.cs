using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player != null)
        {
            ApplyEffect(player);

            Destroy(gameObject);
        }
    }

    protected abstract void ApplyEffect(
        PlayerController player
    );
}
