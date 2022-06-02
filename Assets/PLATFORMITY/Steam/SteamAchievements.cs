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
            {
                initialized = true;

                SteamUserStats.RequestCurrentStats();

                //Print appID
                AppId_t appId_t = SteamUtils.GetAppID();
                Debug.Log("App ID: " + appId_t);

                //Print username
                string name = SteamFriends.GetPersonaName();
                Debug.Log(name);
            }
        }

        //DEBUG INPUT
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetAchievement("ACH_NORMAL_COMPLETED");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetAchievement("ACH_HARD_COMPLETED");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetAchievement("ACH_HC_COMPLETED");

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SteamUserStats.ResetAllStats(true);
        }
    }

    public void SetAchievement(string achievementName)
    {
        if (!initialized)
            return;

        //Check if achievement is already completed
        SteamUserStats.GetAchievement(achievementName, out bool completed);

        if (!completed)
        {
            //Set achievement completed and store it
            SteamUserStats.SetAchievement(achievementName);
            SteamUserStats.StoreStats();
        }
        else
        {
            Debug.Log(achievementName + ": completed!");
        }
    }
}
