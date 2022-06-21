using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float fallRate = .2f; // seconds/line
    [SerializeField] private float fallDistance = 1f;
    [SerializeField] private float heldInputDelay = 0.1f;
    [SerializeField] private float fastFallRate = 0.05f;
    [SerializeField] private float instantDropFallRate = 0.0001f;
    public bool isFalling = true;
    private float lastFall = 0;
    private float currentFallRate;
    private GridManager gridManager;
    private GameSession gameSession;
    
    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        gameSession = FindObjectOfType<GameSession>();
        // Randomize each gem in the block
        foreach (Transform child in transform)
        {
            Spawner spawner = FindObjectOfType<Spawner>();
            int j = Random.Range(0, spawner.gems.Length);
            Sprite newSprite = spawner.gems[j].gameObject.GetComponent<SpriteRenderer>().sprite;
            //print("Sprite is " + newSprite.name);
            child.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        }

        // Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }

        currentFallRate = fallRate;
    }

    // Update is called once per frame
    void Update()
    {
        moveBlock();
        rotateBlock();
    }

    void moveBlock()
    {
        if (gameSession.isPaused)
            {return;}

        // Move Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if it's valid
            if (isValidGridPos())
                {updateGrid();}
            else
                // If it's not valid, revert.
                {transform.position += new Vector3(1, 0, 0);}
        }
        // Move Right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (isValidGridPos())
                {updateGrid();}
            else
                // If it's not valid, revert.
                {transform.position += new Vector3(-1, 0, 0);}
        }
        // Move Downwards and Fast Fall
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            //|| Time.time - lastFall >= currentFallRate)
        {
            // float counter = 0;
            // while ((Input.GetKey(KeyCode.DownArrow)) & (counter <= heldInputDelay))
            // {
            //     counter += Time.deltaTime;
            //     print(counter);
            // }
            // if (counter >= heldInputDelay)
            // {
            //     currentFallRate = fastFallRate;
            //     print(currentFallRate);
            // }
            // Modify position
            transform.position += new Vector3(0, -fallDistance, 0);

            // Change fallrate
            currentFallRate = fastFallRate;

            // See if valid
            if (isValidGridPos())
                {updateGrid();} 
            else
            {
                // If it's not valid, revert.
                transform.position += new Vector3(0, fallDistance, 0);

                //gridManager.checkForMatch();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                placeBlock();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;

            
            
            print("Later fall speed of " + currentFallRate);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {currentFallRate = fallRate;
                print("Key up fall speed of " + currentFallRate);}

        // Instant Drop
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentFallRate = instantDropFallRate;

            // Modify position
            transform.position += new Vector3(0, -fallDistance, 0);

            // See if valid
            if (isValidGridPos())
                {updateGrid();} 
            else
            {
                // If it's not valid, revert.
                transform.position += new Vector3(0, fallDistance, 0);

                //gridManager.checkForMatch();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                placeBlock();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;

            // if (Input.GetKeyUp(KeyCode.UpArrow))
            //     {currentFallRate = fallRate;}
        }
        // Fall according to currentFallRate
        else if (Time.time - lastFall >= currentFallRate)
        {
            // Modify position
            transform.position += new Vector3(0, -fallDistance, 0);

            // See if valid
            if (isValidGridPos())
                {updateGrid();} 
            else
            {
                // If it's not valid, revert.
                transform.position += new Vector3(0, fallDistance, 0);

                //GridManager.checkForMatch();

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                placeBlock();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
    }

    void rotateBlock()
    {
        if (gameSession.isPaused)
            {return;}
           //* Rotate clockwise
        if ((Input.GetKeyDown(KeyCode.Space)))// || Input.GetKeyDown(KeyCode.E))
        {
            // Pre-rotate children in opposite direction
            foreach (Transform child in transform)
            {
                child.transform.Rotate(0, 0, 90);
            }
            // Rotate
            transform.Rotate(0, 0, -90);

            // See if valid
            if (isValidGridPos())
                {updateGrid();}
            else
                // If it's not valid, try 1 space Left | Right, and
                {// revert if still not valid.
                    // Modify position left 1
                    transform.position += new Vector3(-1, 0, 0);
                    // See if it's valid
                    if (isValidGridPos())
                        {updateGrid(); return;}
                    else
                        // If it's not valid, try Right 1
                    {
                        transform.position += new Vector3(2, 0, 0);
                        if (isValidGridPos())
                            {updateGrid(); return;}
                        else // If still not valid, revert
                            {transform.position += new Vector3(-1, 0, 0);}
                    }

                    foreach (Transform child in transform)
                    {
                        child.transform.Rotate(0, 0, -90);
                    }
                    transform.Rotate(0, 0, 90);
                }
        }

        //* Rotate counterclockwise
        if ((Input.GetKeyDown(KeyCode.LeftShift)))// || Input.GetKeyDown(KeyCode.Q))
        {
            // Pre-rotate children in opposite direction
            foreach (Transform child in transform)
            {
                child.transform.Rotate(0, 0, -90);
            }
            // Rotate
            transform.Rotate(0, 0, 90);

            // See if valid
            if (isValidGridPos())
                {updateGrid();}
            else
                // If it's not valid, try 1 space Left | Right, and
                {// revert if still not valid.
                    // Modify position left 1
                    transform.position += new Vector3(-1, 0, 0);
                    // See if it's valid
                    if (isValidGridPos())
                        {updateGrid(); return;}
                    else
                        // If it's not valid, try Right 1
                    {
                        transform.position += new Vector3(2, 0, 0);
                        if (isValidGridPos())
                            {updateGrid(); return;}
                        else // If still not valid, revert
                            {transform.position += new Vector3(-1, 0, 0);}
                    }

                    foreach (Transform child in transform)
                    {
                        child.transform.Rotate(0, 0, 90);
                    }
                    transform.Rotate(0, 0, -90);
                }
        }
    }

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = GridManager.roundVec2(child.position);

            // Not inside Border?
            if (!GridManager.isInsideBorder(v))
                {return false;}

            // Block in grid cell (and not part of same group)?
            if (GridManager.grid[(int)v.x, (int)v.y] != null &&
                GridManager.grid[(int)v.x, (int)v.y].parent != transform)
                {return false;}
        }

        return true;
    }

    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < GridManager.gridHeight; y++)
            for (int x = 0; x < GridManager.gridWidth; x++)
                if (GridManager.grid[x, y] != null)
                    if (GridManager.grid[x, y].parent == transform)
                        GridManager.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = GridManager.roundVec2(child.position);
            GridManager.grid[(int)v.x, (int)v.y] = child;
        }        
    }
    private void placeBlock()
    {
        //Set block in place and cause gems to fall if they can
        isFalling = false;

        foreach (Transform child in transform)
        {
            child.GetComponent<Gem>().isFalling = false;
        }

        this.transform.DetachChildren();

        Destroy(this.gameObject);
    }

    IEnumerator delayedLeftHold()
    {
        yield return new WaitForSeconds(heldInputDelay);
    }
    IEnumerator delayedRightHold()
    {
        yield return new WaitForSeconds(heldInputDelay);
    }
    IEnumerator delayedDownHold()
    {
        yield return new WaitForSeconds(heldInputDelay);
    }
}
