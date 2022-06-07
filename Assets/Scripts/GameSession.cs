using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;
    
    private void Awake() 
    {
        SetupSingleton();    
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

    public void ResetGameSession()
    {
        Destroy(gameObject);
    }
}