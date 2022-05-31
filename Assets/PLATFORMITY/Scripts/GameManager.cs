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

    [Header("Gradient Background")]
    public SpriteRenderer background;
    public Sprite whiteBackground;
    public Sprite blackBackground;

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
                if (sprite != background)
                {
                    if (sprite.color == Color.black) sprite.color = Color.white;
                    else if (sprite.color == Color.white) sprite.color = Color.black;
                }           
            }

            background.sprite = whiteBackground;
        }
        else
        {
            background.sprite = blackBackground;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If in editor, skip this level
        if (Input.GetKeyDown(KeyCode.P))
        {
            TimerManager.timerMan.LevelCompleted();
            LoadLevel(0, false);
        }
            

#if UNITY_EDITOR

#endif
    }

    int GetNextScene(bool reload)
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = (reload) ? currentScene : currentScene + 1;
        if (nextScene > CurrentLevelPool) nextScene = 0;

        return nextScene;
    }

    public void LoadLevel(float delay, bool reload)
    {
        string levelPrefsName = "Level" + GetNextScene(reload);

        //Check if level is last of its chapter
        string sceneName = SceneManager.GetActiveScene().name;
        int level = (int)char.GetNumericValue(sceneName[0]);
        int chapter = (int)char.GetNumericValue(sceneName[2]);

        bool timerOn = (PlayerPrefs.GetInt("Timer", 0) == 1) ? true : false;
        if ((level == 5 && !reload)) //Chapter completed
        {
            //Check if not completed before & if so, award player with coins
            if (!reload)
            {
                //First time completed level, add coins & save them
                if (PlayerPrefs.GetInt(levelPrefsName) == 0)
                {
                    int coins = PlayerPrefs.GetInt("Coins");
                    coins += 50;

                    PlayerPrefs.SetInt("Coins", coins);
                }

                //Save completion & save time
                PlayerPrefs.SetInt(levelPrefsName, 1);
            }

            //Timer, skip chapter completion screen if timer isn't on
            if (timerOn)
            {
                //Show completion screen on hud
                HudManager.hudMan.UpdateCompletion(chapter);
            }
            else
            {
                //Load to next level
                StartCoroutine(LoadScene(delay, reload));
            }           
        }
        else
        {
            //Load to next level
            StartCoroutine(LoadScene(delay, reload));
        }

        //Unlock next level
        PlayerPrefs.SetInt(levelPrefsName, 1);
    }

    public void LoadNextChapter()
    {
        StartCoroutine(LoadScene(0, false));
    }

    IEnumerator LoadScene(float delay, bool reload)
    {
        yield return new WaitForSeconds(delay);

        int nextScene = GetNextScene(reload);
        SceneManager.LoadScene(nextScene);
    }  
}
