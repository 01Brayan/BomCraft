using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float duration = 0.4f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Daño al jugador
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage();
            }
        }

        // Muerte de enemigos
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy =
                other.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}
