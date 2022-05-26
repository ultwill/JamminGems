using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 25;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public static void checkForMatch()
    {
        //Check if 3+ adjacent gems are of the same type
    }
    public static void deleteGems()
    {
        //To be filled in with a function to delete matched gems
    }

    public static void dropGems()
    {
        //Make gems that have no placed gem beneath them to fall until they hit
        // either the bottom or a placed gem
    }
}
