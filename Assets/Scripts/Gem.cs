using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private float gemFallRate = 0.05f; // Units / frame
    private static Gem selectedGem;
    private static Gem gem2;
    private SpriteRenderer spriteRenderer;
    public Vector2Int intPosition;
    public bool isFalling = true;
    [SerializeField] private Transform posBelow;
    GridManager gridManager;
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = FindObjectOfType<GridManager>();
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
        spriteRenderer.color = Color.grey;
    }

    public void Unselect()
    {
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if(selectedGem != null)
        {
            if (selectedGem == this)
                {return;}

            selectedGem.Unselect();
            gem2 = this;
            //print(Vector2Int.Distance(selectedGem.intPosition, gem2.intPosition));
            if (Vector2Int.Distance(selectedGem.intPosition, gem2.intPosition) == 1)
            {
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

        if ((this.transform.parent == null) && !isObjectBeneath(_posBelow))
            {isFalling = true;}
        else if ((this.transform.parent == null) && isObjectBeneath(_posBelow)) // Snap to Grid
        {
            isFalling = false;
            transform.position = new Vector3 (intPosition.x, intPosition.y, transform.position.z);
            //updateGrid();
            gridManager.checkForMatch();
        }

        if ((this.transform.parent == null) && isFalling && !gameSession.isPaused)
        {
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
        // Remove old children from grid
        for (int y = 0; y < GridManager.gridHeight; y++)
            for (int x = 0; x < GridManager.gridWidth; x++)
                if (GridManager.grid[x, y] != null)
                    if (GridManager.grid[x, y] == transform)
                        GridManager.grid[x, y] = null;

        // Add new children to grid
            GridManager.grid[intPosition.x, intPosition.y] = this.transform;  
    }

    public static Vector2Int roundVec2(Vector2 v)
    {
        return new Vector2Int((int)Mathf.Round(v.x), (int)Mathf.Round(v.y));
    }
}
