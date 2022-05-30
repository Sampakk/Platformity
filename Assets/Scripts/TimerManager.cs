using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager timerMan;
    HudManager hud;

    string timerPrefsName = "highScores";
    float timer = 0;
    bool timerGoing = false;
    string allTimes = "";
    float timeRounded;
    string oldTimes = "";

    // Start is called before the first frame update
    void Start()
    {
        if (timerMan == null) timerMan = this;
        else Destroy(gameObject);

        allTimes = PlayerPrefs.GetString(timerPrefsName);

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

        if (timerGoing)
        {
            timer += Time.deltaTime;

            timeRounded = (float)Math.Round(timer, 2);

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

        hud = FindObjectOfType<HudManager>();

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1) //Not menu or shop
        {
            string savedTime = GetSavedTime(SceneManager.GetActiveScene().name);
            UpdateHSHud(savedTime);

            timerGoing = true;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 0) oldTimes = PlayerPrefs.GetString(timerPrefsName);
    }

    public string GetSavedTime(string sceneName)
    {
        string[] times = PlayerPrefs.GetString(timerPrefsName).Split(' ');
        if (times.Contains(sceneName))
        {
            int index = Array.IndexOf(times, sceneName);
            string savedTime = times[index + 1];
            return savedTime;
        }
        else return "0";
    }

    public void LevelCompleted()
    {
        string[] times = PlayerPrefs.GetString(timerPrefsName).Split(' ');

        if (times.Contains(SceneManager.GetActiveScene().name))
        {
            float savedTime = float.Parse(GetSavedTime(SceneManager.GetActiveScene().name));
            if (timeRounded < savedTime)
            {
                string allTimesUpdated = allTimes.Replace(savedTime.ToString(), timeRounded.ToString());
                allTimes = allTimesUpdated;
                SaveTimer(allTimesUpdated);
            }
        }
        else
        {
            allTimes += SceneManager.GetActiveScene().name.ToString() + " " + Math.Round(timeRounded, 2) + " ";
            SaveTimer(allTimes);
        }
        StopTimer();
    }

    public string GetOldTime(string sceneName)
    {
        string[] oldTimers = oldTimes.Split(' ');

        if (oldTimers.Contains(sceneName))
        {
            int index = Array.IndexOf(oldTimers, sceneName);
            string savedOldTime = oldTimers[index + 1];
            return savedOldTime;
            //if (float.Parse(savedOldTime) < float.Parse(GetSavedTime(sceneName)))
            //{
            //    return savedOldTime.ToString();
            //}
            //else return GetSavedTime(sceneName);
        }
        else return "0";


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