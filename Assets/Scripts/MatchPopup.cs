using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchPopup : MonoBehaviour
{
    private string scoreText = "";
    [SerializeField] Sprite[] sprites;
    public static MatchPopup Instance;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textBox;
    private Animator animator;
    private GridManager gridManager;

    void Awake()
    {
        Instance = this;
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        textBox = this.GetComponent<TextMeshPro>();
        animator = this.GetComponent<Animator>();
        gridManager = FindObjectOfType<GridManager>();
    }

    public void scorePopup(int matches, int score)
    {
        if (matches == 3)
        {
            spriteRenderer.sprite = sprites[0];
            textBox.text = "+" + score;
        }
        else if (matches == 4)
        {
            spriteRenderer.sprite = sprites[1];
            textBox.text = "+" + score;
        }
        else if (matches == 5)
        {
            spriteRenderer.sprite = sprites[2];
            textBox.text = "+" + score;
        }
        else if (matches == 6)
        {
            spriteRenderer.sprite = sprites[3];
            textBox.text = "+" + score;
        }
        else if (matches == 7)
        {
            spriteRenderer.sprite = sprites[4];
            textBox.text = "+" + score;
        }
        else if (matches == 8)
        {
            spriteRenderer.sprite = sprites[5];
            textBox.text = "+" + score;
        }
        else if (matches == 9)
        {
            spriteRenderer.sprite = sprites[6];
            textBox.text = "+" + score;
        }
        else if (matches >= 10)
        {
            spriteRenderer.sprite = sprites[7];
            textBox.text = "+" + score;
        }

        animator.Play("Popup");
        print("Matched");
    }
}
