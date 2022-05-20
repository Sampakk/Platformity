using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    TimerManager timerManager;

    public GameObject playerPrefab;
    public Vector3Int spawnPosition;
    public Vector3Int endPosition;
    public int CurrentLevelPool;

    // Start is called before the first frame update
    void Start()
    {
        timerManager = FindObjectOfType<TimerManager>();

        //Singleton
        if (game == null)
            game = this;

        //Spawn player
        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.GetComponent<PlayerMovement>().LevelAnimation(true);

        //Hide cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        //Flip colors if scene index is even
        if (SceneManager.GetActiveScene().buildIndex % 2 != 0)
        {
            foreach (SpriteRenderer sprite in FindObjectsOfType<SpriteRenderer>())
            {
                if (sprite.color == Color.black) sprite.color = Color.white;
                else if (sprite.color == Color.white) sprite.color = Color.black;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If in editor, skip this level
        if (Input.GetKeyDown(KeyCode.P))
            LoadLevel(0, false);

#if UNITY_EDITOR

#endif
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
        if (nextScene > CurrentLevelPool) nextScene = 0;

        //Save this level completed
        if (!reload)
        {
            string levelPrefsName = "Level" + nextScene;

            //First time completed level, add coins & save them
            if (PlayerPrefs.GetInt(levelPrefsName) == 0)
            {
                int coins = PlayerPrefs.GetInt("Coins");
                coins += 10;

                PlayerPrefs.SetInt("Coins", coins);
            }

            //Save completion & save time
            PlayerPrefs.SetInt(levelPrefsName, 1);
            timerManager.LevelCompleted();
        }     

        SceneManager.LoadScene(nextScene);
    }
}
