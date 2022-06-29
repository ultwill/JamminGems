using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float gemFallRate = 0.05f; //measured in Units/frame
    private static Gem selectedGem; // static so their is only one at a time
    private static Gem gem2; // ^^
    private SpriteRenderer spriteRenderer;
    public Vector2Int intPosition;
    public bool isFalling = true;
    public bool isAnimating = false;
    [SerializeField] private Transform posBelow; // The position where we check if something is directly below
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        gameSession = FindObjectOfType<GameSession>();

    }

    // Update is called once per frame
    void Update()
    {
        intPosition = roundVec2(transform.position);
        Fall();
    }
    public void Select()
    {
        SoundManager.Instance.PlaySound(0); // Play Select sound
        spriteRenderer.color = Color.grey;
    }

    public void Unselect()
    {
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if(selectedGem != null) // if a gem is already selected
        {
            if (selectedGem == this) // if the selected gem is already this one
                {return;}

            selectedGem.Unselect();
            gem2 = this;
            if (Vector2Int.Distance(selectedGem.intPosition, gem2.intPosition) == 1)
            { // if the selected gems are adjacent
                GridManager gridManager = FindObjectOfType<GridManager>();
                gridManager.SwapTiles(gem2.intPosition, selectedGem.intPosition);
                selectedGem = null;
            }
            else
            {
                selectedGem = this;
                Select();
            }
        }
        else
        {
            selectedGem = this;
            Select();
        }
    }

    private void Fall()
    {
        Vector3 _posBelow = posBelow.position;

        if ((this.transform.parent == null) && !isObjectBeneath(_posBelow)) // if gem is not part of a block, and nothing's below
            {isFalling = true;}
        else if ((this.transform.parent == null) && isObjectBeneath(_posBelow))
        {
            isFalling = false; // stop falling and snap to grid
            transform.position = new Vector3 (intPosition.x, intPosition.y, transform.position.z);
        }

        if ((this.transform.parent == null) && isFalling && !gameSession.isPaused)
        { // if it can fall, then fall according to gemFallrate
            transform.position += new Vector3(0, -gemFallRate, 0);
            updateGrid();
        }
    }

    private bool isObjectBeneath(Vector3 position)
    {
        bool isBeneath = Physics2D.Linecast(position, position);
        return isBeneath;
    }

    void updateGrid()
    {
        // Remove transform from old section of the grid
        for (int y = 0; y < GridManager.gridHeight; y++)
            for (int x = 0; x < GridManager.gridWidth; x++)
                if (GridManager.grid[x, y] != null)
                    if (GridManager.grid[x, y] == transform)
                        GridManager.grid[x, y] = null;

        // Add transform to updated section of the grid
            GridManager.grid[intPosition.x, intPosition.y] = this.transform;  
    }

    public static Vector2Int roundVec2(Vector2 v)
    {
        return new Vector2Int((int)Mathf.Round(v.x), (int)Mathf.Round(v.y));
    }

    void DeleteSelf() //* This is called at the end of the Match animation
    {
        Destroy(this.gameObject);
    }
}
