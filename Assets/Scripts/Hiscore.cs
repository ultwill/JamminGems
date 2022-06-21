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
        if (gameSession.GetScore() >= gameSession.GetHiscore())
            {gameSession.SetHisccore(gameSession.GetScore());}
        hiscoreText.text = gameSession.GetHiscore().ToString();
    }
}
