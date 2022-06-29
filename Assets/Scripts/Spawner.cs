using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] gems;
    [SerializeField] GameObject[] blocks;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        spawnNext();
    }

    public void spawnNext()
    {
        // Random block layout from array
        int i = Random.Range(0, blocks.Length);
        GameObject newBlock = blocks[i];

        // Randomize each gem sprite in block
        foreach (Transform child in newBlock.transform)
        {
            int j = Random.Range(0, gems.Length);
            Sprite newSprite = gems[j].gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
            child.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
        }

        // Spawn new block at Spawner's current Position
        Instantiate(newBlock, transform.position, Quaternion.identity);
    }
}
