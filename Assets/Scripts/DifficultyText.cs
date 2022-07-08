using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyText : MonoBehaviour
{
    TextMeshProUGUI difficultyText;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        difficultyText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession.difficulty == 0)
            {difficultyText.text = "Chill";}
        else if (gameSession.difficulty == 1)
            {difficultyText.text = "Normal";}
        else if (gameSession.difficulty == 2)
            {difficultyText.text = "Hard";}
    }
}
