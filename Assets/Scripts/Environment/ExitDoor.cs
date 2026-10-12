using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Sprites de escotilla")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    [SerializeField] private bool isOpen = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    public void OpenDoor()
    {
        if (isOpen)
            return;

        isOpen = true;

        UpdateVisual();

        Debug.Log("¡La puerta se ha abierto!");
    }

    private void UpdateVisual()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return;

        if (isOpen)
            spriteRenderer.sprite = openSprite;
        else
            spriteRenderer.sprite = closedSprite;

        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            "Algo entro a la puerta: "
            + other.gameObject.name
        );

        if (!isOpen)
        {
            Debug.Log(
                "La puerta sigue cerrada."
            );

            return;
        }

        if (!other.CompareTag("Player"))
        {
            Debug.Log(
                "El objeto no tiene Tag Player."
            );

            return;
        }

        Debug.Log(
            "Player entro a la puerta abierta."
        );

        LevelManager levelManager =
            FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.CompleteLevel();
        }
        else
        {
            Debug.LogError(
                "No se encontro LevelManager."
            );
        }
    }
}
