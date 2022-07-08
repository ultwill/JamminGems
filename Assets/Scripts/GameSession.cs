using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public float fallRate = 0.3f;
    public float easyFallRate = 0.3f; // seconds / line dropped
    public float normalFallRate = 0.2f; // seconds / line dropped
    public float hardFallRate = 0.1f; // seconds / line dropped
    public int difficulty = 0; // 0 = Easy, 1 = Normal, 2 = Hard
    private int score = 0;
    private int hiscore = 0;
    private int easyHiscore = 0;
    private int normalHiscore = 0;
    private int hardHiscore = 0;
    public bool isPaused = false;

    private void Awake() 
    {
        SetupSingleton();
        // if (difficulty == 0)
        //     {fallRate = easyFallRate;}
        // else if (difficulty == 1)
        //     {fallRate = normalFallRate;
        //     print(difficulty);}
        // else if (difficulty == 2)
        //     {fallRate = hardFallRate;}
        LoadHiscores();
    }

    private void Update()
    {
        if (difficulty == 0)
            {fallRate = easyFallRate;}
        else if (difficulty == 1)
            {fallRate = normalFallRate;}
        else if (difficulty == 2)
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
    public int GetEasyHiscore() {return easyHiscore;}
    public int GetNormalHiscore() {return normalHiscore;}
    public int GetHardHiscore() {return hardHiscore;}
    public void SetHisccore(int score)
    {
        hiscore = score;
    }

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
        score = 0;
        ResumeGame();
    }

    public void GameOver()
    {
        if (difficulty == 0) // Easy
        {
            if (score > easyHiscore)
            {
                easyHiscore = score;
                SaveEasyHiscore();
            }
        }
        if (difficulty == 1) // Normal
        {
            if (score > normalHiscore)
            {
                normalHiscore = score;
                SaveNormalHiscore();
            }
        }
        if (difficulty == 2) // Hard
        {
            if (score > hardHiscore)
            {
                hardHiscore = score;
                SaveHardHiscore();
            }
        }

        if (score > hiscore)
            {hiscore = score;}
    }

    public void SaveEasyHiscore()
    {
        PlayerPrefs.SetInt("Easy Hiscore", easyHiscore);
    }
    public void SaveNormalHiscore()
    {
        PlayerPrefs.SetInt("Normal Hiscore", normalHiscore);
    }
    public void SaveHardHiscore()
    {
        PlayerPrefs.SetInt("Hard Hiscore", hardHiscore);
    }

    public void LoadHiscores()
    {
        easyHiscore = PlayerPrefs.GetInt("Easy Hiscore", 0); // 0 is the default value
        normalHiscore = PlayerPrefs.GetInt("Normal Hiscore", 0);
        hardHiscore = PlayerPrefs.GetInt("Hard Hiscore", 0);
    }
}