using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformMaker : MonoBehaviour
{
    Camera cam;
    PlayerMovement player;

    public Sprite[] blockSprites;

    [Header("Cursor / aiming")]
    public GameObject targetBlockPrefab;
    public GameObject spriteMaskPrefab;

    [Header("Adding blocks")]
    public GameObject particlePrefab;
    public GameObject blockPrefab;
    public int maxBlocks = 3;

    [Header("Effects")]
    public GameObject blockDestroyEffectPrefab;

    List<GameObject> usedBlocks = new List<GameObject>();
    GameObject targetBlock;
    GameObject spriteMask;

    void Awake()
    {
        //Destroy this on first levels because they are tutorials
        if (SceneManager.GetActiveScene().buildIndex < 7)
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Find components
        cam = FindObjectOfType<Camera>();
        player = FindObjectOfType<PlayerMovement>();

        //Instantiate cursor objects
        targetBlock = Instantiate(targetBlockPrefab, Vector3.zero, Quaternion.identity);
        spriteMask = Instantiate(spriteMaskPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Get necessary vectors
        Vector3 mousePosInWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosInWorld.z = -0.5f; //Towards camera so cursor is over blocks

        Vector3 spriteMaskPosInWorld = mousePosInWorld;
        spriteMaskPosInWorld.x += 0.5f;
        spriteMaskPosInWorld.y -= 0.5f;
        spriteMask.transform.position = spriteMaskPosInWorld;

        //Snap mouse position
        mousePosInWorld.x = Mathf.RoundToInt(mousePosInWorld.x) + 0.5f;
        mousePosInWorld.y = Mathf.RoundToInt(mousePosInWorld.y) - 0.5f;

        targetBlock.transform.position = mousePosInWorld;

        //Check if mouse is over something
        Vector2 mousePos2D = new Vector2(mousePosInWorld.x, mousePosInWorld.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //Add block
        if (Input.GetMouseButtonDown(0))
        {
            //Get speed and direction
            float particleSpeed = 20f;
            Vector3 playerCenter = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
            Vector2 lookDirection = mousePosInWorld - playerCenter;

            //Play audio
            AudioManager.audioMan.PlayThrowSound();

            //Instantiate particle and add force
            GameObject particle = Instantiate(particlePrefab, playerCenter, Quaternion.identity);
            particle.GetComponent<Rigidbody2D>().AddForce(lookDirection.normalized * particleSpeed, ForceMode2D.Impulse);
            particle.GetComponent<Rigidbody2D>().AddTorque(25f);

            //Calculate how long it will take particle to reach target position
            float delay = lookDirection.magnitude / particleSpeed;
            Destroy(particle, delay);

            if (hit.collider != null && !hit.collider.isTrigger) //Mouse over something
            {
                FadeBlock fadeBlock = hit.collider.GetComponent<FadeBlock>();
                if (fadeBlock != null)
                {
                    StartCoroutine(FadeBlock(fadeBlock, delay));
                }
            }
            else //Block can be placed
            {
                StartCoroutine(AddBlock(mousePosInWorld, delay));
            }
        }
    }

    IEnumerator FadeBlock(FadeBlock fadeBlock, float delay)
    {
        yield return new WaitForSeconds(delay);

        //Fade out the block
        fadeBlock.FadeOut();
    }

    IEnumerator AddBlock(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        //If all blocks are used, destroy oldest one
        if (usedBlocks.Count >= maxBlocks)
        {
            GameObject oldestBlock = usedBlocks[0];
            usedBlocks.RemoveAt(0);

            //Particle effect on destroy
            GameObject blockDestroyEffect = Instantiate(blockDestroyEffectPrefab, oldestBlock.transform.position, Quaternion.identity);
            Destroy(blockDestroyEffect, 1.5f);

            Destroy(oldestBlock);
        }

        //Add new block to list
        GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
        usedBlocks.Add(newBlock);

        //Update all blocks sprites
        if (usedBlocks.Count == 1) //Only 1 block used
        {
            usedBlocks[0].GetComponent<SpriteRenderer>().sprite = blockSprites[0]; //Newest & only block
        }
        else if (usedBlocks.Count == 2) //2 blocks used
        {
            usedBlocks[0].GetComponent<SpriteRenderer>().sprite = blockSprites[1]; //New
            usedBlocks[1].GetComponent<SpriteRenderer>().sprite = blockSprites[0]; //Newest
        }
        else if (usedBlocks.Count >= 3) //All 3 blocks used
        {
            usedBlocks[0].GetComponent<SpriteRenderer>().sprite = blockSprites[2]; //Old
            usedBlocks[1].GetComponent<SpriteRenderer>().sprite = blockSprites[1]; //New
            usedBlocks[2].GetComponent<SpriteRenderer>().sprite = blockSprites[0]; //Newest
        }     
    }

    public void DestroyTarget()
    {
        Destroy(targetBlock);
        Destroy(spriteMask);
        Destroy(gameObject);
    }
}
