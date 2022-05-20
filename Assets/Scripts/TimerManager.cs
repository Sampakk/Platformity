using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager timerMan;
    HudManager hud;
    float timer = 0;
    bool timerGoing = false;

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

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            timerGoing = true;
        }
    }

    public void LevelCompleted()
    {
        string allTimes = SceneManager.GetActiveScene().name.ToString() + " , " + timer + " , ";
        Debug.Log(allTimes);
        //PlayerPrefs.SetString(levelPrefsName, 1);
        StopTimer();
    }

    public void StopTimer()
    {
        timerGoing = false;
        timer = 0;
    }

    void SaveTimer(string allTimes)
    {

    }

}