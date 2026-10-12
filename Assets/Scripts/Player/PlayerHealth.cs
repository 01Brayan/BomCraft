using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vidas")]
    [SerializeField] private int maxLives = 3;

    [Header("Invulnerabilidad")]
    [SerializeField] private float invulnerabilityTime = 1.5f;

    private int currentLives;
    private bool isInvulnerable = false;

    private Vector3 spawnPosition;

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentLives = maxLives;

        // Guardamos la posición inicial del jugador como spawn
        spawnPosition = transform.position;

        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Log("Vidas actuales: " + currentLives);
    }

    public void TakeDamage()
    {
        if (isInvulnerable)
            return;

        currentLives--;

        Debug.Log("Jugador recibió daño. Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            GameOver();
            return;
        }

        Respawn();
    }

    private void Respawn()
    {
        if (playerController != null)
        {
            playerController.RespawnAt(spawnPosition);
        }
        else
        {
            transform.position = spawnPosition;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;

        // Parpadeo sencillo mientras es invulnerable
        while (elapsed < invulnerabilityTime)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(0.15f);

            elapsed += 0.15f;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvulnerable = false;
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
