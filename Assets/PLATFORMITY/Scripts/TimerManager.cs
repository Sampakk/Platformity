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
    Scene[] scenes;

    // Start is called before the first frame update
    void Start()
    {
        //Cleans up old highscores
        CleanupHighscores();

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
            string firstFour = new string(savedTime.Take(4).ToArray());
            if (savedTime.Length > 3)
            {
                return HSCleanupFunction(firstFour);
            }
            

            else
            {
                return HSCleanupFunction(savedTime);
            }
        }
        else return "0";
    }

    public void LevelCompleted()
    {
        string[] times = PlayerPrefs.GetString(timerPrefsName).Split(' ');

        if (times.Contains(SceneManager.GetActiveScene().name))
        {
            try
            {
                float savedTime = float.Parse(GetSavedTime(SceneManager.GetActiveScene().name));
                if (timeRounded < savedTime || savedTime == 0)
                {
                    string allTimesUpdated = allTimes.Replace(savedTime.ToString(), timeRounded.ToString());
                    allTimes = allTimesUpdated;
                    SaveTimer(allTimesUpdated);
                }
            }
            catch
            {

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
            return HSCleanupFunction(savedOldTime);
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

    void CleanupHighscores()
    {
        //Splits by " "
        string[] times = PlayerPrefs.GetString(timerPrefsName).Split(' ');
        string correctedTimes = "";

        foreach (string s in times)
        {
            correctedTimes += HSCleanupFunction(s) + " ";
        }
        //push corrected times back to playerprefs
        PlayerPrefs.SetString(timerPrefsName, correctedTimes);
    }

    string HSCleanupFunction(string s)
    {

        int freq = s.Count(f => (f == '.'));

        // If string contains more than 2 '.', split them and join them together so only the numbers before, after and the split itself remains (example 32.56.7.45 -> 32.56)
        if (freq >= 2)
        {
            string[] timearray = s.Split('.');
            string result = timearray[0] + "." + timearray[1];
            return result;

        }
        else return s;
    }
}

