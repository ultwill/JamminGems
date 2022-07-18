using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Color activatedColor; // The color to change ability icons to when they're active
    [SerializeField] Color cooldownColor; // The color to change ability icons to when they're on cooldown
    public static int gridWidth = 10;
    public static int gridHeight = 25;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    public bool swapAbilityActive = false;
    private bool swapAbilityOnCooldown = false;
    public bool timeAbilityActive = false;
    private bool timeAbilityOnCooldown = false;
    private float delayafterSwap = 0.01f; //Slight delay to allow animation to play
    private float swapDelayReference; // A reference point to see when a swap is possible
    private float superswapDuration = 12f;
    private float superswapCooldown = 42f;
    [SerializeField] float swapEasyDuration = 12f;
    [SerializeField] float swapNormalDuration = 9f;
    [SerializeField] float swapHardDuration = 5f;
    [SerializeField] float swapEasyCooldown = 42f;
    [SerializeField] float swapNormalCooldown = 54f;
    [SerializeField] float swapHardCooldown = 65f;
    private float timestopDuration = 12f;
    private float timestopCooldown = 42f;
    [SerializeField] float timeEasyDuration = 12f;
    [SerializeField] float timeNormalDuration = 9f;
    [SerializeField] float timeHardDuration = 5f;
    [SerializeField] float timeEasyCooldown = 42f;
    [SerializeField] float timeNormalCooldown = 54f;
    [SerializeField] float timeHardCooldown = 65f;
    private GameSession gameSession;
    [SerializeField] GameObject dropIndicator;
    
    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        if (gameSession.difficulty == 0)
        {
            superswapDuration = swapEasyDuration;
            superswapCooldown = swapEasyCooldown;

            timestopDuration = timeEasyDuration;
            timestopCooldown = timeEasyCooldown;
        }
        else if (gameSession.difficulty == 1)
        {
            superswapDuration = swapNormalDuration;
            superswapCooldown = swapNormalCooldown;

            timestopDuration = timeNormalDuration;
            timestopCooldown = timeNormalCooldown;
        }
        else if (gameSession.difficulty == 2)
        {
            superswapDuration = swapHardDuration;
            superswapCooldown = swapHardCooldown;

            timestopDuration = timeHardDuration;
            timestopCooldown = timeHardCooldown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checkForMatch();
        handleAbilityInputs();
        moveDropIndicator();
    }
    void LateUpdate()
    {
        checkForMatch(); // In LateUpdate so it doesn't interfere with animations
    }

    private void handleAbilityInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && !swapAbilityActive && !swapAbilityOnCooldown)
        {
            swapAbilityActive = true;
            SpriteRenderer swapIconRenderer = transform.Find("Swap Icon").GetComponent<SpriteRenderer>();
            swapIconRenderer.color = activatedColor;
            StartCoroutine(swapAbilityDuration());
        }

         if (Input.GetKeyDown(KeyCode.Q) && !timeAbilityActive && !timeAbilityOnCooldown)
        {//! Time icon is currently hacked together and may need to change
            timeAbilityActive = true;
            GameObject timeIcon = transform.Find("Timestop Icon").gameObject;
            SpriteRenderer timeIconRenderer = timeIcon.transform.Find("Circle").gameObject.GetComponent<SpriteRenderer>();
            timeIconRenderer.color = activatedColor;
            StartCoroutine(timeAbilityDuration());
        }
    }

    private IEnumerator swapAbilityDuration()
    {
        StartCoroutine(swapAbilityCooldown());
        yield return new WaitForSecondsRealtime(superswapDuration);
        SpriteRenderer swapIconRenderer = transform.Find("Swap Icon").GetComponent<SpriteRenderer>();
        swapIconRenderer.color = cooldownColor;
        swapAbilityActive = false;
        swapAbilityOnCooldown = true;
    }

    private IEnumerator swapAbilityCooldown()
    {
        yield return new WaitForSecondsRealtime(superswapCooldown);
        SpriteRenderer swapIconRenderer = transform.Find("Swap Icon").GetComponent<SpriteRenderer>();
        swapIconRenderer.color = Color.white;
        swapAbilityOnCooldown = false;
        print("Swap cooldown ended");
    }

    private IEnumerator timeAbilityDuration()
    {
        gameSession.PauseGame();
        StartCoroutine(timeAbilityCooldown());
        yield return new WaitForSecondsRealtime(timestopDuration);
        GameObject timeIcon = transform.Find("Timestop Icon").gameObject;
        SpriteRenderer timeIconRenderer = timeIcon.transform.Find("Circle").GetComponent<SpriteRenderer>();
        timeIconRenderer.color = cooldownColor;
        timeAbilityActive = false;
        gameSession.ResumeGame();
        timeAbilityOnCooldown = true;
    }

    private IEnumerator timeAbilityCooldown()
    {
        yield return new WaitForSecondsRealtime(timestopCooldown);
        GameObject timeIcon = transform.Find("Timestop Icon").gameObject;
        SpriteRenderer timeIconRenderer = timeIcon.transform.Find("Circle").GetComponent<SpriteRenderer>();
        timeIconRenderer.color = Color.white;
        timeAbilityOnCooldown = false;
        print("Timestop cooldown ended");
    }

    public static Vector2Int roundVec2(Vector2 v)
    {
        return new Vector2Int((int)Mathf.Round(v.x), (int)Mathf.Round(v.y));
    }

    public static bool isInsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public void SwapTiles(Vector2Int gem1Position, Vector2Int gem2Position)
    {
        swapDelayReference = Time.time; // Set reference point
        Gem gem1 = GetGemAt(gem1Position.x, gem1Position.y);
        SpriteRenderer renderer1 = gem1.GetComponentInChildren<SpriteRenderer>();

        Gem gem2 = GetGemAt(gem2Position.x, gem2Position.y);
        SpriteRenderer renderer2 = gem2.GetComponentInChildren<SpriteRenderer>();
        
        Sprite temp = renderer1.sprite; // Swap sprites
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;
        if ((!swapAbilityActive) && (checkForMatch() == false)) //if no match is made AND Swap Ability is inactive, reset the Swap
        {
            renderer2.sprite = renderer1.sprite;
            renderer1.sprite = temp;
            return;
        }
        if (swapAbilityActive || checkForMatch()) // if the Swap is OK, animate it according to their  relative positions
        {
            if (gem1.transform.position - gem2.transform.position == Vector3.left)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Right");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Left");
                SoundManager.Instance.PlaySound(1); // Play Swap sound
            }
            else if (gem1.transform.position - gem2.transform.position == Vector3.right)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Left");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Right");
                SoundManager.Instance.PlaySound(1); // Play Swap sound
            }
            else if (gem1.transform.position - gem2.transform.position == Vector3.up)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Down");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Up");
                SoundManager.Instance.PlaySound(1); // Play Swap sound
            }
            else if (gem1.transform.position - gem2.transform.position == Vector3.down)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Up");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Down");
                SoundManager.Instance.PlaySound(1); // Play Swap sound
            }
        }
    }
    Gem GetGemAt(int column, int row)
    {
        if (column < 0 || column >= gridWidth // if outside grid, return null
            || row < 0 || row >= gridHeight)
            {return null;}

        Collider2D intersecting = Physics2D.OverlapCircle(new Vector2 (column, row), 0.1f);
        // Check if there is something there
        if (intersecting == null)
            {return null;}
        else 
        {
            Gem gem = intersecting.GetComponentInParent<Gem>();
            return gem;
        }
    }

    public bool checkForMatch()
    {
        HashSet<Gem> matchedGems = new HashSet<Gem>();
        for (int row = 0; row < gridHeight; row++) // Check from left to right, bottom to top
        {
            for (int column = 0; column < gridWidth; column++)
            {
                Gem currentGem = GetGemAt(column, row);
                if ((currentGem == null) || currentGem.isFalling)
                    {continue;} // Check the next one
                
                List<Gem> horizontalMatches = FindColumnMatchForTile(column, row, currentGem);
                List<Gem> verticalMatches = FindRowMatchForTile(column, row, currentGem);
                if ((horizontalMatches.Count >= 2)) // if 2+ matching gems are in the same row
                {
                    matchedGems.Add(currentGem);
                    matchedGems.UnionWith(horizontalMatches);
                }

                 
                if (verticalMatches.Count >= 2) // if 2+ matching gems are in the same column
                {
                    matchedGems.Add(currentGem);
                    matchedGems.UnionWith(verticalMatches);
                }
            }
        }
        if (matchedGems.Count >= 3) //* This batches all the mmatches throughout the grid this frame
        {
            handleMatchedGems(matchedGems);
            return true;
        }
        else
            {return false;}
    }

    List<Gem> FindColumnMatchForTile(int column, int row, Gem currentGem)
    {
        List<Gem> result = new List<Gem>();
        for (int i = column + 1; i < gridWidth; i++)
        {
            Gem nextGem = GetGemAt(i, row);
            if ((nextGem == null) || nextGem.isFalling)
                {break;}
            if (nextGem.GetComponentInChildren<SpriteRenderer>().sprite != currentGem.GetComponentInChildren<SpriteRenderer>().sprite)
                {break;}
            if (nextGem.GetComponentInChildren<SpriteRenderer>().sprite == currentGem.GetComponentInChildren<SpriteRenderer>().sprite)
                {result.Add(nextGem);}
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

            if (nextGem.GetComponentInChildren<SpriteRenderer>().sprite != currentGem.GetComponentInChildren<SpriteRenderer>().sprite)
                {break;}
            if (nextGem.GetComponentInChildren<SpriteRenderer>().sprite == currentGem.GetComponentInChildren<SpriteRenderer>().sprite)
                {result.Add(nextGem);}
        }
        return result;
    }
    public void handleMatchedGems(HashSet<Gem> matchedGems)
    {
        int numAnimating = 0;
        foreach (Gem gem in matchedGems)
        {
            if (gem.isAnimating)
                numAnimating++;
        }
        if ((numAnimating > 0) || ((Time.time - swapDelayReference) < delayafterSwap))
            {return;}

        SoundManager.Instance.PlaySound(2); //* Clip 2 is the match sound
        if (matchedGems.Count == 3)
        {
            gameSession.AddToScore(3);
        }
        else if (matchedGems.Count == 4)
        {
            gameSession.AddToScore(20);
        }
        else if (matchedGems.Count == 5)
        {
            gameSession.AddToScore(50);
        }
        else if (matchedGems.Count == 6)
        {
            gameSession.AddToScore(300);
        }
        else if (matchedGems.Count == 7)
        {
            gameSession.AddToScore(1400);
        }
        else if (matchedGems.Count == 8)
        {
            gameSession.AddToScore(8000);
        }
        else if (matchedGems.Count == 9)
        {
            gameSession.AddToScore(90000);
        }
        else if (matchedGems.Count >= 10)
        {
            gameSession.AddToScore(100000 * matchedGems.Count);
        }

        foreach (Gem gem in matchedGems)
        {
            if (!gem.isAnimating)
            {
                gem.GetComponent<Animator>().Play("Match"); //!This animation ends with destroying the matched gem
            }
        }
    }
    private void moveDropIndicator()
    {
        dropIndicator.transform.position = FindObjectOfType<Block>().transform.position;
    }
}