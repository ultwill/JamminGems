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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnNext()
    {
        // Random Index
        int i = Random.Range(0, blocks.Length);
        GameObject newBlock = blocks[i];
        // Spawn Group at Spawner's current Position
        Instantiate(newBlock, transform.position, Quaternion.identity);

        // foreach (Transform child in newBlock.transform)
        // {
        //     int j = Random.Range(0, gems.Length);
        //     Sprite newSprite = gems[j].gameObject.GetComponent<SpriteRenderer>().sprite;
        //     print("Sprite is " + newSprite.name);
        //     child.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        // }

        
    }
}
