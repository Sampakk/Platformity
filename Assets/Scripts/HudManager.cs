using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HudManager : MonoBehaviour
{
    public static HudManager hudMan;
    TimerManager timer;
    public Canvas canvas;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        if (hudMan == null) hudMan = this;
        else Destroy(gameObject);

        timer = FindObjectOfType<TimerManager>();

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        //Update level text
        string levelName = SceneManager.GetActiveScene().name;
        levelText.text = "Level: " + levelName;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }

        //Back to menu with esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            timer.StopTimer();
        }
    }
}
