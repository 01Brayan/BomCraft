using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float fuseTime = 2.5f;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int explosionRange = 1;

    [Header("Deteccion")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask bombLayer;

    private PlayerController player;
    private bool hasExploded = false;

    public void Initialize(PlayerController owner, int range)
    {
        player = owner;
        explosionRange = range;
    }

    private void Start()
    {
        Invoke(nameof(Detonate), fuseTime);
    }

    public void Detonate()
    {
        if (hasExploded)
            return;

        hasExploded = true;

        CancelInvoke(nameof(Detonate));

        CreateExplosion(transform.position);

        ExplodeDirection(Vector2.up);
        ExplodeDirection(Vector2.down);
        ExplodeDirection(Vector2.left);
        ExplodeDirection(Vector2.right);

        if (player != null)
        {
            player.NotifyBombDestroyed();
        }

        Destroy(gameObject);
    }

    private void ExplodeDirection(Vector2 direction)
    {
        for (int i = 1; i <= explosionRange; i++)
        {
            Vector2 targetPosition = (Vector2)transform.position + direction * i;

            Collider2D obstacle = Physics2D.OverlapBox(
                targetPosition,
                Vector2.one * 0.8f,
                0f,
                obstacleLayer
            );

            if (obstacle != null)
            {
                DestructibleTilemap destructible =
                    obstacle.GetComponent<DestructibleTilemap>();

                if (destructible != null &&
                    destructible.HasTileAt(targetPosition))
                {
                    CreateExplosion(targetPosition);
                    destructible.DestroyTileAt(targetPosition);
                }

                break;
            }

            Collider2D bombCollider = Physics2D.OverlapBox(
                targetPosition,
                Vector2.one * 0.6f,
                0f,
                bombLayer
            );

            if (bombCollider != null)
            {
                Bomb otherBomb = bombCollider.GetComponent<Bomb>();

                if (otherBomb != null && otherBomb != this)
                {
                    CreateExplosion(targetPosition);
                    otherBomb.Detonate();
                }

                break;
            }

            CreateExplosion(targetPosition);
        }
    }

    private void CreateExplosion(Vector3 position)
    {
        Instantiate(
            explosionPrefab,
            position,
            Quaternion.identity
        );
    }
}
