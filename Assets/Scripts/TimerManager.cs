using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    string playerPrefsName = "highScores";
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
            timerGoing = true;
        }
    }

    public void LevelCompleted()
    {
        string[] times = allTimes.Split(',');
        
        if (times.Contains(SceneManager.GetActiveScene().name))
        {
            /*int index = Array.IndexOf(times, SceneManager.GetActiveScene().name);
            //Debug.Log(times[index + 1] + " nice");

            float savedTime = float.Parse(times[index + 1]);

            if (timer < savedTime)
            {
                Debug.Log("new hs");
                allTimes += SceneManager.GetActiveScene().name.ToString() + "," + Math.Round(timer, 2) + ",";
            }
            else Debug.Log("shit time");*/
        }
        else
        {
            allTimes += SceneManager.GetActiveScene().name.ToString() + "," + Math.Round(timer, 2) + ",";
        }

        SaveTimer(allTimes);
        StopTimer();
    }

    public void StopTimer()
    {
        timerGoing = false;
        timer = 0;
    }

    void SaveTimer(string allTimes)
    {
        /*string[] times = allTimes.Split(',');

        if (times.Contains(SceneManager.GetActiveScene().name))
        {
            int index = Array.IndexOf(times, SceneManager.GetActiveScene().name);
            Debug.Log(times[index + 1] + " nice");  
        }*/
    }

}
//PlayerPrefs.SetString(playerPrefsName, allTimes);
//allTimes += SceneManager.GetActiveScene().name.ToString() + "," + Math.Round(timer, 2) + ",";