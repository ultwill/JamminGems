using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float fallRate = .2f; // seconds/line
    [SerializeField] private float fallDistance = 1f; // the distance the block falls each step
    [SerializeField] private float heldInputDelay = 0.1f; //! held input not yet implemented
    [SerializeField] private float fastFallRate = 0.05f;
    [SerializeField] private float instantDropFallRate = 0.0001f;
    public bool isFalling = true;
    private bool instantDropping = false; //* Plyer cannot move or rotate after initiating Instant Drop (W/Up arrow)
    private float lastFall = 0;
    private float currentFallRate;
    private GridManager gridManager;
    private GameSession gameSession;
    
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        gameSession = FindObjectOfType<GameSession>();
        fallRate = gameSession.fallRate; // sync fallRate with the fall rate set by Game Session
    }

    // Start is called before the first frame update
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!isValidGridPos())
        {
            print("GAME OVER");
            StartCoroutine(gameOver());
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
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !instantDropping)
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
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !instantDropping)
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
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !instantDropping)
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

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                placeBlock();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {currentFallRate = fallRate;}

        // Instant Drop
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentFallRate = instantDropFallRate;
            instantDropping = true;
            // Modify position
            transform.position += new Vector3(0, -fallDistance, 0);

            // See if valid
            if (isValidGridPos())
                {updateGrid();} 
            else
            {
                // If it's not valid, revert.
                transform.position += new Vector3(0, fallDistance, 0);

                // Spawn next Group
                FindObjectOfType<Spawner>().spawnNext();

                placeBlock();

                // Disable script
                enabled = false;
            }

            lastFall = Time.time;
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
        if (gameSession.isPaused || instantDropping)
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

                    foreach (Transform child in transform) //Revert
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

                    foreach (Transform child in transform) //Revert
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

    private IEnumerator gameOver()
    {
        transform.position += new Vector3(0,1,0); // move out of the way
        gameSession.PauseGame();
        yield return new WaitForSecondsRealtime(1f);
        gameSession.GameOver();
        gameSession.ResumeGame();
        Menu.LoadGameOverScene();
    }

    IEnumerator delayedLeftHold() //! Unimplemented
    {
        yield return new WaitForSeconds(heldInputDelay);
    }
    IEnumerator delayedRightHold() //! Unimplemented
    {
        yield return new WaitForSeconds(heldInputDelay);
    }
    IEnumerator delayedDownHold() //! Unimplemented
    {
        yield return new WaitForSeconds(heldInputDelay);
    }
}
