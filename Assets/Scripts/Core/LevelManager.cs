using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private ExitDoor exitDoor;

    private int enemiesAlive = 0;
    private bool hasKey = false;
    private bool levelCompleted = false;

    private void Start()
    {
        exitDoor =
            FindFirstObjectByType<ExitDoor>();

        if (exitDoor == null)
        {
            Debug.LogError(
                "No se encontro un ExitDoor en esta escena."
            );
        }
    }

    public void RegisterEnemy()
    {
        enemiesAlive++;

        Debug.Log(
            "Enemigo registrado. Total: "
            + enemiesAlive
        );
    }

    public void EnemyDefeated()
    {
        enemiesAlive--;

        if (enemiesAlive < 0)
            enemiesAlive = 0;

        Debug.Log(
            "Enemigos restantes: "
            + enemiesAlive
        );

        CheckExitConditions();
    }

    public void CollectKey()
    {
        hasKey = true;

        Debug.Log(
            "¡Llave recogida!"
        );

        CheckExitConditions();
    }

    private void CheckExitConditions()
    {
        if (enemiesAlive <= 0 && hasKey)
        {
            if (exitDoor != null)
            {
                exitDoor.OpenDoor();
            }
            else
            {
                Debug.LogError(
                    "No se encontro ExitDoor."
                );
            }
        }
    }

    public void CompleteLevel()
    {
        if (levelCompleted)
            return;

        levelCompleted = true;

        Debug.Log(
            "¡Nivel completado!"
        );

        int currentSceneIndex =
            SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex =
            currentSceneIndex + 1;

        SceneManager.LoadScene(
            nextSceneIndex
        );
    }
    public bool HasKey()
    {
        return hasKey;
    }
}
