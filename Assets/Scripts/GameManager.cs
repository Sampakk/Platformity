using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Vector3Int spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn player
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        //Hide cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
