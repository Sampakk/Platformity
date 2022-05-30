using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    public static SteamAchievements achievements;

    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        //Setup singleton
        if (achievements == null)
        {
            achievements = this;
        }
        else
        {
            if (achievements != this)
                Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Wait until steam manager is initialized
        if (!initialized)
        {
            if (SteamManager.Initialized)
                initialized = true;

            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
        }

        if (initialized)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetAchievement("NORMAL_COMPLETED");

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SetAchievement("HARD_COMPLETED");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                SetAchievement("HC_COMPLETED");
        }
    }

    public void SetAchievement(string achievementName)
    {
        if (!initialized)
            return;

        //Set achievement completed and store it
        SteamUserStats.SetAchievement(achievementName);
        SteamUserStats.StoreStats();


        bool isCompleted = false;
        SteamUserStats.GetAchievement(achievementName, out isCompleted);
        Debug.Log("Completed: " + isCompleted);
    }
}
