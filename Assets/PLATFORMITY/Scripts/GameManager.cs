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

        //Hide cursor but not in shop scene
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }    

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
        //Load to next scene on normal mode
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = (reload) ? currentScene : currentScene + 1;
        if (nextScene > CurrentLevelPool) nextScene = 0;

        //Check for gamemodes as they are different
        if (reload)
        {
            int gamemode = PlayerPrefs.GetInt("Gamemode", 0);
            if (gamemode == 1) //Hard
            {
                int levelIndex = currentScene - 1; //First level is 1
                if (levelIndex > 5) //Other than chapter 1
                {
                    int chapter = Mathf.CeilToInt((float)levelIndex / 5);
                    nextScene = chapter * 5 - 5;
                    nextScene += 2;
                }
                else
                {
                    nextScene = 2; //First levels build index
                }
            }
            else if (gamemode == 2) //HC
            {
                nextScene = 2;
            }
        }     

        return nextScene;
    }

    public void LoadLevel(float delay, bool reload)
    {
        int gamemode = PlayerPrefs.GetInt("Gamemode", 0);
        int nextScene = GetNextScene(reload);
        string levelPrefsName = "Level" + nextScene;
        string hardLevelPrefsName = "HardLevel" + nextScene;

        //Check if level is last of its chapter
        string sceneName = SceneManager.GetActiveScene().name;
        int level = (int)char.GetNumericValue(sceneName[0]);
        string chapter = "";
        for (int i = 0; i < sceneName.Length; i++) if (i > 1) chapter += sceneName[i];

        if ((level == 5 && !reload)) //Chapter completed
        {
            //Check if not completed before & if so, award player with coins
            if (!reload)
            {
                //First time completed level
                if (PlayerPrefs.GetInt(levelPrefsName) == 0)
                {
                    //Add 50 coins to player
                    int coins = PlayerPrefs.GetInt("Coins");
                    coins += 50;

                    PlayerPrefs.SetInt("Coins", coins);

                    //Achievements
                    string chapterAchievement = "ACH_CHAPTER" + chapter + "_NORMAL";
                    SteamAchievements.achievements.SetAchievement(chapterAchievement);
                } 
            }

            //Show completion screen on hud
            HudManager.hudMan.UpdateCompletion(chapter);
        }
        else
        {
            //Load to next level
            StartCoroutine(LoadScene(delay, reload));
        }

        //Unlock next level
        if (!reload)
        {
            if (gamemode == 0) //Normal mode
            {
                //Save completion & save time
                PlayerPrefs.SetInt(levelPrefsName, 1);
            }
            else if (gamemode == 1) //Hard mode
            {
                //Save completion & save time
                PlayerPrefs.SetInt(hardLevelPrefsName, 1);
            }
        }         

        //Completed the last level, completion achievement
        if (nextScene == 0)
        {
            if (gamemode == 0) //Normal
            {
                SteamAchievements.achievements.SetAchievement("ACH_NORMAL_COMPLETED");
            }
            else if (gamemode == 1) //Hard
            {
                SteamAchievements.achievements.SetAchievement("ACH_HARD_COMPLETED");
            }
            else if (gamemode == 2) //HC
            {
                SteamAchievements.achievements.SetAchievement("ACH_HC_COMPLETED");
            }           
        }
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
