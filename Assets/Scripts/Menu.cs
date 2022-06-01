using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void DisplayPauseMenu()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        PauseGame();
    }

    public void ExitPauseMenu()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ResumeGame();
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        //SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadScene(scene.name);
        ResumeGame();

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
