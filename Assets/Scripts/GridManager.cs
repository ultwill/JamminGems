using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GridManager : MonoBehaviour
{
    [SerializeField] Color activatedColor;
    [SerializeField] Color cooldownColor;
    public static int gridWidth = 10;
    public static int gridHeight = 25;
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    private bool swapAbilityActive = false;
    private bool swapAbilityOnCooldown = false;
    private bool timeAbilityActive = false;
    private bool timeAbilityOnCooldown = false;
    private float delayafterSwap = 0.01f; //Slight delay to allow animation to play
    private float swapDelayReference;
    private float delayafterMatch = 0.01f; //Slight delay to allow animation to play
    private float matchDelayReference;
    [SerializeField] float superswapDuration = 5f;
    [SerializeField] float timestopDuration = 5f;
    [SerializeField] float swapCooldown = 30f;
    [SerializeField] float timestopCooldown = 30f;
    private GameSession gameSession;
    
    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        //checkForMatch();
        handleAbilityInputs();
    }
    void LateUpdate()
    {
        checkForMatch();
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
        {
            timeAbilityActive = true;
            GameObject timeIcon = transform.Find("Timestop Icon").gameObject;
            SpriteRenderer timeIconRenderer = timeIcon.transform.Find("Circle").gameObject.GetComponent<SpriteRenderer>();
            timeIconRenderer.color = activatedColor;
            StartCoroutine(timeAbilityDuration());
            gameSession.PauseGame();
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
        yield return new WaitForSecondsRealtime(swapCooldown);
        SpriteRenderer swapIconRenderer = transform.Find("Swap Icon").GetComponent<SpriteRenderer>();
        swapIconRenderer.color = Color.white;
        swapAbilityOnCooldown = false;
        print("Swap cooldown ended");
    }

    private IEnumerator timeAbilityDuration()
    {
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
        swapDelayReference = Time.time;
        Gem gem1 = GetGemAt(gem1Position.x, gem1Position.y);
        SpriteRenderer renderer1 = gem1.GetComponentInChildren<SpriteRenderer>();

        Gem gem2 = GetGemAt(gem2Position.x, gem2Position.y);
        SpriteRenderer renderer2 = gem2.GetComponentInChildren<SpriteRenderer>();
        
        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;
        if ((!swapAbilityActive) && (checkForMatch() == false)) //if no match is made AND Swap Ability is inactive, reset the Swap
        {
            renderer2.sprite = renderer1.sprite;
            renderer1.sprite = temp;
            return;
        }
        if (swapAbilityActive || checkForMatch())
        {
            if (gem1.transform.position - gem2.transform.position == Vector3.left)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Right");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Left");
            }
            else if (gem1.transform.position - gem2.transform.position == Vector3.right)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Left");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Right");
            }
            else if (gem1.transform.position - gem2.transform.position == Vector3.up)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Down");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Up");
            }
            else if (gem1.transform.position - gem2.transform.position == Vector3.down)
            {
                Animator animator1 = gem1.GetComponent<Animator>();
                animator1.Play("Swap Up");
                Animator animator2 = gem2.GetComponent<Animator>();
                animator2.Play("Swap Down");
            }
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
            //print(gem.GetComponent<SpriteRenderer>().sprite.name + " found at [" + column + "," + row + "]");
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
            matchDelayReference = Time.time;
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
        int numSwapping = 0;
        foreach (Gem gem in matchedGems)
        {
            if (gem.isAnimating)
                numSwapping++;
        }
        if ((Time.time - swapDelayReference < delayafterSwap) || (numSwapping > 0))
            {return;}

        foreach (Gem gem in matchedGems)
        {
            if (!gem.isAnimating)
            {
                gem.GetComponent<Animator>().Play("Match");
                // if (Time.time - matchDelayReference > delayafterMatch)
                // {
                //     Destroy(gem.gameObject);
                //     gameSession.AddToScore(100);
                // }
            }
        }
    }
}