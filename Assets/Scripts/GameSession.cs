using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public float fallRate = 0.4f;
    public float easyFallRate = 0.4f; // seconds / line dropped
    public float normalFallRate = 0.2f; // seconds / line dropped
    public float hardFallRate = 0.1f; // seconds / line dropped
    public int difficulty = 0; // 0 = Easy, 1 = Normal, 2 = Hard
    private int score = 0;
    private int hiscore = 0;
    public bool isPaused = false;
    private void Awake() 
    {
        SetupSingleton();
        if (difficulty == 0)
            {fallRate = easyFallRate;}
        else if (difficulty == 1)
            {fallRate = normalFallRate;
            print(difficulty);}
        else if (difficulty == 2)
            {fallRate = hardFallRate;}
    }

    private void Update()
    {
        if (difficulty == 0)
            {fallRate = easyFallRate;}
        if (difficulty == 1)
            {fallRate = normalFallRate;}
        if (difficulty == 2)
            {fallRate = hardFallRate;}
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
    public int GetHiscore() {return hiscore;}

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
        if (score > hiscore)
            {hiscore = score;}
        score = 0;
    }

    public void GameOver()
    {
        if (score > hiscore)
            {hiscore = score;}
    }
}