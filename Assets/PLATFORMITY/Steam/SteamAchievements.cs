using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    public static SteamAchievements achievements;

    public bool initialized = false;

    // Awake is called before start
    void Awake()
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

        initialized = false;

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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetAchievement("ACH_NORMAL_COMPLETED");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetAchievement("ACH_HARD_COMPLETED");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetAchievement("ACH_HC_COMPLETED");

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SteamUserStats.ResetAllStats(true);
#endif
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

        //Check all for completionist achievement
        if (achievementName != "ACH_COMPLETIONIST")
            StartCoroutine(CheckIfCompletedAll());
    }

    public bool HasCompletedAchievement(string achievementName)
    {
        //Check if achievement is already completed
        SteamUserStats.GetAchievement(achievementName, out bool completed);

        return completed;
    }

    IEnumerator CheckIfCompletedAll()
    {
        //Wait for few frames so the game wont crash :D
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //Check completions
        bool completedNormal = HasCompletedAchievement("ACH_NORMAL_COMPLETED");
        bool completedHard = HasCompletedAchievement("ACH_HARD_COMPLETED");
        bool completedHc = HasCompletedAchievement("ACH_HC_COMPLETED");

        if (!completedNormal || !completedHard || !completedHc)
            yield break;

        //Check hats
        bool boughtHat = HasCompletedAchievement("ACH_BOUGHT_HAT");
        bool boughtAllHats = HasCompletedAchievement("ACH_ALL_HATS_BOUGHT");

        if (!boughtHat || !boughtAllHats)
            yield break;

        //Completed all
        SetAchievement("ACH_COMPLETIONIST");
    }
}
