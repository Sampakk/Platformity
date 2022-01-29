using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadLevel(float delay, bool reload)
    {
        StartCoroutine(LoadScene(delay, reload));
    }

    IEnumerator LoadScene(float delay, bool reload)
    {
        yield return new WaitForSeconds(delay);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = (reload) ? currentScene : currentScene + 1;

        SceneManager.LoadScene(nextScene);
    }
}
