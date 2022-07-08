using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hiscore : MonoBehaviour
{
    TextMeshProUGUI hiscoreText;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        hiscoreText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.difficulty == 0) // Easy
        {
            if (gameSession.GetScore() >= gameSession.GetEasyHiscore())
                {gameSession.SaveEasyHiscore();}
            hiscoreText.text = gameSession.GetEasyHiscore().ToString();
        }
        if (gameSession.difficulty == 1) // Normal
        {
            if (gameSession.GetScore() >= gameSession.GetNormalHiscore())
                {gameSession.SaveNormalHiscore();}
            hiscoreText.text = gameSession.GetNormalHiscore().ToString();
        }
        if (gameSession.difficulty == 2) // Hard
        {
            if (gameSession.GetScore() >= gameSession.GetHardHiscore())
                {gameSession.SaveHardHiscore();}
            hiscoreText.text = gameSession.GetHardHiscore().ToString();
        }
    }
}
