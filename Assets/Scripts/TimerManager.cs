using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    string timerPrefsName = "highScores";
    public static TimerManager timerMan;
    HudManager hud;
    float timer = 0;
    bool timerGoing = false;
    string allTimes = "";

    // Start is called before the first frame update
    void Start()
    {
        if (timerMan == null) timerMan = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerGoing)
        {
            timer += Time.deltaTime;

            if (hud != null)
            {
                hud.timerText.text = "Time: " + Math.Round(timer,2);
            }
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Level Loaded");
        //Debug.Log(scene.name);
        //Debug.Log(mode);

        hud = FindObjectOfType<HudManager>();

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log(GetSavedTime(SceneManager.GetActiveScene().name));
            string savedTime = GetSavedTime(SceneManager.GetActiveScene().name);
            //Debug.Log(savedTime);
            UpdateHSHud(savedTime);

            timerGoing = true;
        }
    }

    string GetSavedTime(string sceneName)
    {
        string[] times = PlayerPrefs.GetString(timerPrefsName).Split(',');
        if (times.Contains(sceneName))
        {
            int index = Array.IndexOf(times, SceneManager.GetActiveScene().name);
            string savedTime = times[index + 1];
            return savedTime;
        }
        else return "none";

    }

    public void LevelCompleted()
    {
        string[] times = PlayerPrefs.GetString(timerPrefsName).Split(',');

        if (times.Contains(SceneManager.GetActiveScene().name))
        {
            float savedTime = float.Parse(GetSavedTime(SceneManager.GetActiveScene().name));

            if (timer < savedTime && timer != 0)
            {
                string allTimesUpdated = allTimes.Replace(savedTime.ToString(), timer.ToString());
                SaveTimer(allTimesUpdated);
            }
            else Debug.Log("shit time");
        }
        else
        {
            allTimes += SceneManager.GetActiveScene().name.ToString() + "," + Math.Round(timer, 2) + ",";
            SaveTimer(allTimes);
        }
        StopTimer();
        //Debug.Log(PlayerPrefs.GetString(timerPrefsName));
    }

    public void StopTimer()
    {
        timerGoing = false;
        timer = 0;
    }

    void SaveTimer(string timesToSave)
    {
        PlayerPrefs.SetString(timerPrefsName, timesToSave);
    }

    void UpdateHSHud(string highScore)
    {
        hud.highScoreText.text = "Highscore: " + highScore;
    }
}
//PlayerPrefs.SetString(playerPrefsName, allTimes);
//allTimes += SceneManager.GetActiveScene().name.ToString() + "," + Math.Round(timer, 2) + ",";