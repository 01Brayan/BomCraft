using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveDuration = 0.15f;

    [Header("Deteccion de obstaculos")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask bombLayer;

    [Header("Bombas")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private int maxBombs = 1;
    [SerializeField] private int explosionRange = 1;

    [Header("Limites de Power-Ups")]
    [SerializeField] private int maxBombLimit = 3;
    [SerializeField] private int maxExplosionRange = 3;
    [SerializeField] private float minMoveDuration = 0.11f;

    private int activeBombs = 0;
    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
            return;

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            direction = Vector2.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            direction = Vector2.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction = Vector2.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction = Vector2.right;

        if (direction != Vector2.zero)
        {
            TryMove(direction);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBomb();
        }
    }

    private void TryMove(Vector2 direction)
    {
        Vector2 targetPosition = (Vector2)transform.position + direction;

        bool obstacleBlocked = Physics2D.OverlapBox(
            targetPosition,
            Vector2.one * 0.8f,
            0f,
            obstacleLayer
        );

        bool bombBlocked = Physics2D.OverlapBox(
            targetPosition,
            Vector2.one * 0.6f,
            0f,
            bombLayer
        );

        if (!obstacleBlocked && !bombBlocked)
        {
            StartCoroutine(MoveTo(targetPosition));
        }
    }

    private IEnumerator MoveTo(Vector2 targetPosition)
    {
        isMoving = true;

        Vector2 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / moveDuration;

            transform.position = Vector2.Lerp(
                startPosition,
                targetPosition,
                t
            );

            yield return null;
        }

        transform.position = targetPosition;

        isMoving = false;
        CheckEnemyContact();
    }

    private void PlaceBomb()
    {
        if (activeBombs >= maxBombs)
            return;

        Vector2 bombPosition = new Vector2(
            Mathf.Round(transform.position.x * 2f) / 2f,
            Mathf.Round(transform.position.y * 2f) / 2f
        );

        // Evita colocar otra bomba en la misma celda
        Collider2D existingBomb = Physics2D.OverlapBox(
            bombPosition,
            Vector2.one * 0.5f,
            0f,
            bombLayer
        );

        if (existingBomb != null)
            return;

        GameObject bombObject = Instantiate(
            bombPrefab,
            bombPosition,
            Quaternion.identity
        );

        Bomb bomb = bombObject.GetComponent<Bomb>();

        if (bomb != null)
        {
            bomb.Initialize(this, explosionRange);
        }

        activeBombs++;
    }

    public void NotifyBombDestroyed()
    {
        activeBombs--;

        if (activeBombs < 0)
            activeBombs = 0;
    }
    public void RespawnAt(Vector3 spawnPosition)
    {
        StopAllCoroutines();

        isMoving = false;

        transform.position = spawnPosition;
    }

    private void CheckEnemyContact()
    {
        Collider2D enemy = Physics2D.OverlapBox(
            transform.position,
            Vector2.one * 0.6f,
            0f,
            LayerMask.GetMask("Enemy")
        );

        if (enemy != null)
        {
            PlayerHealth health =
                GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage();
            }
        }
    }
    public void IncreaseExplosionRange()
    {
        if (explosionRange < maxExplosionRange)
        {
            explosionRange++;

            Debug.Log(
                "Fire Up recogido. Alcance actual: "
                + explosionRange
            );
        }
    }

    public void IncreaseMaxBombs()
    {
        if (maxBombs < maxBombLimit)
        {
            maxBombs++;

            Debug.Log(
                "Bomb Up recogido. Bombas maximas: "
                + maxBombs
            );
        }
    }

    public void IncreaseSpeed()
    {
        if (moveDuration > minMoveDuration)
        {
            moveDuration -= 0.02f;

            if (moveDuration < minMoveDuration)
                moveDuration = minMoveDuration;

            Debug.Log(
                "Speed Up recogido. Move Duration: "
                + moveDuration
            );
        }
    }
}
