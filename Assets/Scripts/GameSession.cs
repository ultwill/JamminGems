using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public float fallRate = 0.25f; // seconds / line dropped
    private int score = 0;
    public bool isPaused = false;
    private void Awake() 
    {
        SetupSingleton();    
    }

    private void Update()
    {
        // Block currentBlock = FindObjectOfType<Block>();
        // currentBlock.fallRate = fallRate;
    }
    private void SetupSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    public int GetScore() {return score;}

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ResetGameSession()
    {
        Destroy(gameObject);
    }
}