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
    public TextMeshProUGUI highScoreText;

    [Header("Completion")]
    public GameObject completionRoot;
    public TextMeshProUGUI completeText;

    [Header("Levels")]
    public GameObject levelTimesRoot;
    public TextMeshProUGUI level1TimeText;
    public TextMeshProUGUI level2TimeText;
    public TextMeshProUGUI level3TimeText;
    public TextMeshProUGUI level4TimeText;
    public TextMeshProUGUI level5TimeText;

    // Start is called before the first frame update
    void Start()
    {
        if (hudMan == null) hudMan = this;
        else Destroy(gameObject);

        //Get timer
        timer = FindObjectOfType<TimerManager>();

        //Hide completion root
        completionRoot.SetActive(false);

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

    public void UpdateCompletion(int chapter)
    {
        //Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Show completion root
        completionRoot.SetActive(true);

        //Update completion text
        completeText.text = "You have completed chapter " + chapter;

        //Update level times
        level1TimeText.text = TimerManager.timerMan.GetSavedTime("1-" + chapter);
        level2TimeText.text = TimerManager.timerMan.GetSavedTime("2-" + chapter);
        level3TimeText.text = TimerManager.timerMan.GetSavedTime("3-" + chapter);
        level4TimeText.text = TimerManager.timerMan.GetSavedTime("4-" + chapter);
        level5TimeText.text = TimerManager.timerMan.GetSavedTime("5-" + chapter);

        //Start fading in the levels
        StartCoroutine(FadeInLevels());
    }

    IEnumerator FadeInLevels()
    {
        CanvasGroup[] canvasGroups = levelTimesRoot.GetComponentsInChildren<CanvasGroup>();

        //Hide all canvas groups
        foreach (CanvasGroup canvasGroup in canvasGroups)
            canvasGroup.alpha = 0;

        //Fade in levels one by one
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += 2f * Time.deltaTime;

                yield return null;
            }
        }
    }

    public void LoadNextChapter()
    {
        GameManager.game.LoadNextChapter();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        completionRoot.SetActive(false);
    }
}
