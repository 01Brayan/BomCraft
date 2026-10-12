using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        LevelManager levelManager =
            FindFirstObjectByType<LevelManager>();

        if (levelManager != null)
        {
            levelManager.CollectKey();
        }

        Destroy(gameObject);
    }
}
