using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMaker : MonoBehaviour
{
    Camera cam;
    PlayerMovement player;

    [Header("Cursor / aiming")]
    public GameObject targetBlockPrefab;
    public GameObject spriteMaskPrefab;

    [Header("Adding blocks")]
    public GameObject particlePrefab;
    public GameObject blockPrefab;
    public int maxBlocks = 5;

    List<GameObject> usedBlocks = new List<GameObject>();
    GameObject targetBlock;
    GameObject spriteMask;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        player = FindObjectOfType<PlayerMovement>();

        targetBlock = Instantiate(targetBlockPrefab, Vector3.zero, Quaternion.identity);
        spriteMask = Instantiate(spriteMaskPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Get necessary vectors
        Vector3 mousePosInWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosInWorld.z = 0;

        spriteMask.transform.position = mousePosInWorld;

        //Snap mouse position
        mousePosInWorld.x = Mathf.RoundToInt(mousePosInWorld.x) + 0.5f;
        mousePosInWorld.y = Mathf.RoundToInt(mousePosInWorld.y) - 0.5f;

        targetBlock.transform.position = mousePosInWorld;

        //Add block
        if (Input.GetMouseButtonDown(0))
        {
            //Get speed and direction
            float particleSpeed = 20f;
            Vector3 playerCenter = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
            Vector2 lookDirection = mousePosInWorld - playerCenter;

            //Instantiate particle and add force
            GameObject particle = Instantiate(particlePrefab, playerCenter, Quaternion.identity);
            particle.GetComponent<Rigidbody2D>().AddForce(lookDirection.normalized * particleSpeed, ForceMode2D.Impulse);
            particle.GetComponent<Rigidbody2D>().AddTorque(25f);

            //Calculate how long it will take particle to reach target position
            float delay = lookDirection.magnitude / particleSpeed;
            Destroy(particle, delay);

            StartCoroutine(AddBlock(mousePosInWorld, delay));
        }
    }

    IEnumerator AddBlock(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        //If all blocks are used, destroy oldest one
        if (usedBlocks.Count >= maxBlocks)
        {
            GameObject oldestBlock = usedBlocks[0];
            usedBlocks.RemoveAt(0);

            Destroy(oldestBlock);
        }

        //Add new block to list
        GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
        usedBlocks.Add(newBlock);
    }
}
