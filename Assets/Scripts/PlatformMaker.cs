using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMaker : MonoBehaviour
{
    Camera cam;

    public GameObject targetPrefab;
    public GameObject blockPrefab;
    public int maxBlocks = 5;

    List<GameObject> usedBlocks = new List<GameObject>();
    GameObject targetBlock;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();

        targetBlock = Instantiate(targetPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //Get necessary vectors
        Vector2 mousePos = Input.mousePosition;
        Vector3 mousePosInWorld = cam.ScreenToWorldPoint(mousePos);
        mousePosInWorld.x = Mathf.RoundToInt(mousePosInWorld.x) + 0.5f;
        mousePosInWorld.y = Mathf.RoundToInt(mousePosInWorld.y) - 0.5f;
        mousePosInWorld.z = 0;

        //Update target position
        targetBlock.transform.position = mousePosInWorld;

        //Add block
        if (Input.GetMouseButtonDown(0))
        {
            //If all blocks are used, destroy oldest one
            if (usedBlocks.Count >= maxBlocks)
            {
                GameObject oldestBlock = usedBlocks[0];
                usedBlocks.RemoveAt(0);
                Destroy(oldestBlock);

                Debug.Log("Removing oldest block");
            }

            //Add new block to list
            GameObject newBlock = Instantiate(blockPrefab, mousePosInWorld, Quaternion.identity);
            usedBlocks.Add(newBlock);
        }
    }
}
