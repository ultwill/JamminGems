using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchPopup : MonoBehaviour
{
    private string scoreText = "";
    [SerializeField] Sprite[] sprites;
    public static MatchPopup Instance;
    //[SerializeField] 
    private SpriteRenderer[] spriteRenderer;
    private TextMeshPro textBox;
    private Animator animator;
    private GridManager gridManager;

    void Awake()
    {
        Instance = this;
        spriteRenderer = this.GetComponentsInChildren<SpriteRenderer>();
        textBox = this.GetComponent<TextMeshPro>();
        animator = this.GetComponent<Animator>();
        gridManager = FindObjectOfType<GridManager>();
    }

    public void scorePopup(int matches, int score)
    {
        if (matches < 10)
        {
            spriteRenderer[1].sprite = sprites[matches];
            spriteRenderer[2].sprite = null;
            textBox.text = "+" + score + " pts";
        }
        else if (matches >= 10)
        {
            spriteRenderer[1].sprite = sprites[(matches / 10) % 10];
            spriteRenderer[2].sprite = sprites[matches % 10];
            textBox.text = "+" + score + " pts";
        }
        // if (matches == 3)
        // {
        //     spriteRenderer[1].sprite = sprites[3];
        //     spriteRenderer[2].sprite = null;
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 4)
        // {
        //     spriteRenderer[1].sprite = sprites[4];
        //     spriteRenderer[2].sprite = null;
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 5)
        // {
        //     spriteRenderer[1].sprite = sprites[5];
        //     spriteRenderer[2].sprite = null;
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 6)
        // {
        //     spriteRenderer[1].sprite = sprites[6];
        //     spriteRenderer[2].sprite = null;
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 7)
        // {
        //     spriteRenderer[1].sprite = sprites[7];
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 8)
        // {
        //     spriteRenderer[1].sprite = sprites[8];
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 9)
        // {
        //     spriteRenderer[1].sprite = sprites[9];
        //     spriteRenderer[2].sprite = null;
        //     textBox.text = "+" + score;
        // }
        // else if (matches == 10)
        // {
        //     spriteRenderer[1].sprite = sprites[1];
        //     spriteRenderer[2].sprite = sprites[0];
        //     textBox.text = "+" + score;
        // }

        animator.Play("Popup");
        print("Matched");
    }
}
