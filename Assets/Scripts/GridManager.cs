using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 25;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    GameSession gameSession;    
    
    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForMatch();
    }

    public static Vector2Int roundVec2(Vector2 v)
    {
        return new Vector2Int((int)Mathf.Round(v.x), (int)Mathf.Round(v.y));
    }

    public static bool isInsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        Transform tile1 = grid[tile1Position.x, tile1Position.y];
        SpriteRenderer renderer1 = tile1.GetComponent<SpriteRenderer>();

        Transform tile2 = grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer2 = tile2.GetComponent<SpriteRenderer>();
        
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;
        if (checkForMatch() == false) //if no match is made, reset the Swap
        {
            temp = renderer1.sprite;
            renderer1.sprite = renderer2.sprite;
            renderer2.sprite = temp;
        }
    }
    Gem GetGemAt(int column, int row)
    {
        if (column < 0 || column >= gridWidth
            || row < 0 || row >= gridHeight)
            {return null;}

        Collider2D intersecting = Physics2D.OverlapCircle(new Vector2 (column, row), 0.1f);// Physics.OverlapSphere(new Vector3(column, row, 0), 0.1f);
        if (intersecting == null)
            {return null;}
        else 
        {
            Transform tile = grid[column, row];
            Gem gem = intersecting.GetComponentInParent<Gem>();
            print(gem.GetComponent<SpriteRenderer>().sprite.name + " found at [" + column + "," + row + "]");
            return gem;
        }
    }

    public bool checkForMatch()
    {
        HashSet<Gem> matchedGems = new HashSet<Gem>();
        for (int row = 0; row < gridHeight; row++)
        {
            for (int column = 0; column < gridWidth; column++)
            {
                Gem currentGem = GetGemAt(column, row);
                if ((currentGem == null) || currentGem.isFalling)
                    {continue;}
                
                List<Gem> horizontalMatches = FindColumnMatchForTile(column, row, currentGem);
                List<Gem> verticalMatches = FindRowMatchForTile(column, row, currentGem);
                if ((horizontalMatches.Count >= 2))// || (verticalMatches.Count > 0))
                {
                    matchedGems.Add(currentGem);
                    matchedGems.UnionWith(horizontalMatches);
                    //matchedGems.UnionWith(verticalMatches);
                }

                 
                if (verticalMatches.Count >= 2)
                {
                    matchedGems.UnionWith(verticalMatches);
                    matchedGems.Add(currentGem);
                }
            }
        }
        if (matchedGems.Count >= 3)
        {
            handleMatchedGems(matchedGems);
            return true;
        }
        return false;
    }

    List<Gem> FindColumnMatchForTile(int column, int row, Gem currentGem)
    {
        List<Gem> result = new List<Gem>();
        for (int i = column + 1; i < gridWidth; i++)
        {
            Gem nextGem = GetGemAt(i, row);
            if ((nextGem == null) || nextGem.isFalling)
                {break;}
            if (nextGem.GetComponent<SpriteRenderer>().sprite != currentGem.GetComponent<SpriteRenderer>().sprite)
                {print("Test");break;}
            if (nextGem.GetComponent<SpriteRenderer>().sprite == currentGem.GetComponent<SpriteRenderer>().sprite)
                {result.Add(nextGem);print(result.Count + " Horizontal matches");}
        }
        return result;
    }

    List<Gem> FindRowMatchForTile(int column, int row, Gem currentGem)
    {
        List<Gem> result = new List<Gem>();
        for (int i = row + 1; i < gridHeight; i++)
        {
            Gem nextGem = GetGemAt(column, i);
            if ((nextGem == null) || nextGem.isFalling)
                {break;}

            if (nextGem.GetComponent<SpriteRenderer>().sprite != currentGem.GetComponent<SpriteRenderer>().sprite)
                {break;}
            if (nextGem.GetComponent<SpriteRenderer>().sprite == currentGem.GetComponent<SpriteRenderer>().sprite)
                {result.Add(nextGem);print(result.Count + " Vertical matches");}
        }
        return result;
    }
    public void handleMatchedGems(HashSet<Gem> matchedGems)
    {
        foreach (Gem gem in matchedGems)
        {
            Destroy(gem.gameObject);
            gameSession.AddToScore(100);
        }
    }
}