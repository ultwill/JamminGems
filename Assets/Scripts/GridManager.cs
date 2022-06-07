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
    }
    GameObject GetGemAt(int column, int row)
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
            GameObject gem = tile.gameObject;
            print(gem.GetComponent<SpriteRenderer>().sprite.name + " found at [" + column + "," + row + "]");
            return gem;
        }
    }

    public bool checkForMatch()
    {
        HashSet<GameObject> matchedGems = new HashSet<GameObject>();
        for (int row = 0; row < gridHeight; row++)
        {
            for (int column = 0; column < gridWidth; column++)
            {
                GameObject currentGem = GetGemAt(column, row);
                if (currentGem == null)
                    {continue;}
                
                List<GameObject> horizontalMatches = FindColumnMatchForTile(column, row, currentGem);
                List<GameObject> verticalMatches = FindRowMatchForTile(column, row, currentGem);
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

    List<GameObject> FindColumnMatchForTile(int column, int row, GameObject currentGem)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = column + 1; i < gridWidth; i++)
        {
            GameObject nextGem = GetGemAt(i, row);
            if (nextGem == null)
                {break;}
            if (nextGem.GetComponent<SpriteRenderer>().sprite != currentGem.GetComponent<SpriteRenderer>().sprite)
                {print("Test");break;}
            if (nextGem.GetComponent<SpriteRenderer>().sprite == currentGem.GetComponent<SpriteRenderer>().sprite)
                {result.Add(nextGem);print(result.Count + " Horizontal matches");}
        }
        return result;
    }

    List<GameObject> FindRowMatchForTile(int column, int row, GameObject currentGem)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = row + 1; i < gridHeight; i++)
        {
            GameObject nextGem = GetGemAt(column, i);
            if (nextGem == null)
                {break;}

            if (nextGem.GetComponent<SpriteRenderer>().sprite != currentGem.GetComponent<SpriteRenderer>().sprite)
                {break;}
            if (nextGem.GetComponent<SpriteRenderer>().sprite == currentGem.GetComponent<SpriteRenderer>().sprite)
                {result.Add(nextGem);print(result.Count + " Vertical matches");}
        }
        return result;
    }
    public void handleMatchedGems(HashSet<GameObject> matchedGems)
    {
        foreach (GameObject gem in matchedGems)
        {
            Destroy(gem);
            gameSession.AddToScore(100);
        }
            //TODO: Add Score
            print(matchedGems.Count > 0);
    }
}