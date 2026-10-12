using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Corazones")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartDisabled;

    [Header("Llave")]
    [SerializeField] private Image keyIcon;
    [SerializeField] private Sprite keyFull;
    [SerializeField] private Sprite keyDisabled;

    private PlayerHealth playerHealth;
    private LevelManager levelManager;

    private void Start()
    {
        playerHealth =
            FindFirstObjectByType<PlayerHealth>();

        levelManager =
            FindFirstObjectByType<LevelManager>();

        UpdateHUD();
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        UpdateHearts();
        UpdateKey();
    }

    private void UpdateHearts()
    {
        if (playerHealth == null)
            return;

        int currentLives =
            playerHealth.GetCurrentLives();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].sprite =
                    heartFull;
            }
            else
            {
                hearts[i].sprite =
                    heartDisabled;
            }
        }
    }

    private void UpdateKey()
    {
        if (levelManager == null ||
            keyIcon == null)
        {
            return;
        }

        if (levelManager.HasKey())
        {
            keyIcon.sprite =
                keyFull;
        }
        else
        {
            keyIcon.sprite =
                keyDisabled;
        }
    }
}
