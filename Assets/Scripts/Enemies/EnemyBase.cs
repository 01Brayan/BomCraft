using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] protected float moveDuration = 0.5f;

    [Header("Referencias")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Grid grid;

    [Header("Deteccion")]
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected LayerMask bombLayer;

    protected bool isMoving = false;
    protected LevelManager levelManager;

    protected virtual void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        if (grid == null)
        {
            grid = FindFirstObjectByType<Grid>();
        }

        if (grid != null)
        {
            Vector3Int currentCell =
                grid.WorldToCell(transform.position);

            Vector3 centeredPosition =
                grid.GetCellCenterWorld(currentCell);

            centeredPosition.z =
                transform.position.z;

            transform.position =
                centeredPosition;
        }

        levelManager =
            FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.RegisterEnemy();
        }
    }

    protected virtual void Update()
    {
        if (!isMoving)
        {
            FollowPlayer();
        }
    }

    protected void FollowPlayer()
    {
        if (player == null || grid == null)
            return;

        Vector3Int? nextCell =
            GridPathfinder.FindNextStep(
                transform.position,
                player.position,
                grid,
                obstacleLayer,
                bombLayer
            );

        if (nextCell.HasValue)
        {
            Vector3 targetPosition =
                grid.GetCellCenterWorld(
                    nextCell.Value
                );

            targetPosition.z =
                transform.position.z;

            StartCoroutine(
                MoveTo(targetPosition)
            );
        }
        else
        {
            TryRandomMove();
        }
    }

    protected void TryRandomMove()
    {
        Vector3Int currentCell =
            grid.WorldToCell(
                transform.position
            );

        Vector3Int[] directions =
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        for (int i = 0; i < 8; i++)
        {
            Vector3Int direction =
                directions[
                    Random.Range(
                        0,
                        directions.Length
                    )
                ];

            Vector3Int targetCell =
                currentCell + direction;

            bool blocked =
                GridPathfinder.IsCellBlocked(
                    targetCell,
                    grid,
                    obstacleLayer,
                    bombLayer
                );

            if (!blocked)
            {
                Vector3 targetPosition =
                    grid.GetCellCenterWorld(
                        targetCell
                    );

                targetPosition.z =
                    transform.position.z;

                StartCoroutine(
                    MoveTo(targetPosition)
                );

                return;
            }
        }
    }

    protected IEnumerator MoveTo(
        Vector3 targetPosition)
    {
        isMoving = true;

        Vector3 startPosition =
            transform.position;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                elapsed / moveDuration;

            transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            yield return null;
        }

        transform.position =
            targetPosition;

        CheckPlayerContact();

        isMoving = false;
    }

    protected void CheckPlayerContact()
    {
        if (player == null || grid == null)
            return;

        Vector3Int enemyCell =
            grid.WorldToCell(
                transform.position
            );

        Vector3Int playerCell =
            grid.WorldToCell(
                player.position
            );

        if (enemyCell == playerCell)
        {
            PlayerHealth health =
                player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage();
            }
        }
    }

    public void Die()
    {
        if (levelManager != null)
        {
            levelManager.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}
