using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameSession gameSession;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }
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
        gameSession.PauseGame();
    }

    public void ResumeGame()
    {
        gameSession.ResumeGame();
    }

    public void RestartLevel()
    {
        gameSession.ResetGameSession();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        
        ResumeGame();

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadDifficultyScene()
    {
        gameSession.ResetGameSession();
        SceneManager.LoadScene("DifficultySelect");
    }

    public void LoadGameEasy()
    {
        gameSession.difficulty = 0;
        SceneManager.LoadScene("TestLevel1");
    }

    public void LoadGameNormal()
    {
        gameSession.difficulty = 1;
        SceneManager.LoadScene("TestLevel1");
    }

    public void LoadGameHard()
    {
        gameSession.difficulty = 2;
        SceneManager.LoadScene("TestLevel1");
    }
    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("Options");
    }

    public static void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadMainMenuScene()
    {
        gameSession.ResumeGame();
        gameSession.ResetGameSession();
        SceneManager.LoadScene("MainMenu");
    }
}
