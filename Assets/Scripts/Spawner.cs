using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] gems;
    [SerializeField] GameObject[] blocks;
    [SerializeField] Transform blockPreviewTransform;
    private GameObject currentBlock;
    private GameObject nextBlock;
    private GameObject middlemanBlock;
    
    // Start is called before the first frame update
    void Start()
    {
        currentBlock = createRandomBlock();
        nextBlock = createRandomBlock();
        displayNextBlock();
        spawnFirstBlock();
    }

    private GameObject createRandomBlock()
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

        return newBlock;
        
    }

    private void spawnFirstBlock()
    {
        enableBlockSpripts(currentBlock);
        // Spawn new block at Spawner's current Position
        Instantiate(currentBlock, transform.position, Quaternion.identity);
    }
    public void spawnBlock()
    {
        middlemanBlock = nextBlock;
        //middlemanBlock.transform.parent = null;
        currentBlock = middlemanBlock;
        enableBlockSpripts(currentBlock);
        removeOldPreview();
        
        // Spawn new block at Spawner's current Position
        Instantiate(currentBlock, transform.position, Quaternion.identity);
        nextBlock = createRandomBlock();
        if (nextBlock.tag == currentBlock.tag)
        {
            nextBlock = createRandomBlock();
        }
        displayNextBlock();
    }
    private void displayNextBlock()
    {
        disableBlockSpripts(nextBlock);
        // Spawn new block at Spawner's current Position
        Instantiate(nextBlock, blockPreviewTransform.position, Quaternion.identity, blockPreviewTransform);
    }
    private void disableBlockSpripts(GameObject block)
    {
        // Disable movement/collision for the preview
        foreach (var component in block.GetComponents<MonoBehaviour>())
        {
            component.enabled = false; // Disable scripts like movement/collision
        }
    }
    private void enableBlockSpripts(GameObject block)
    {
        //block.SetActive(true);
        // Disable movement/collision for the preview
        foreach (var component in block.GetComponents<MonoBehaviour>())
        {
            component.enabled = true; // Disable scripts like movement/collision
        }
    }
    private void removeOldPreview()
    {
        foreach (Transform child in blockPreviewTransform)
        {
            Destroy(child.gameObject);
        }
    }
}
