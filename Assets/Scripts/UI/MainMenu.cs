using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RetryGame()
    {
        SceneManager.LoadScene("Nivel1");
    }
}
