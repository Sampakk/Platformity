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
    public TextMeshProUGUI[] levelTimeTexts;

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

        //Update all level times
        for (int i = 0; i < levelTimeTexts.Length; i++)
        {
            TextMeshProUGUI timeText = levelTimeTexts[i];

            int level = i + 1;        
            float newTime = float.Parse(TimerManager.timerMan.GetSavedTime(level + "-" + chapter));
            float oldTime = float.Parse(TimerManager.timerMan.GetOldTime(level + "-" + chapter));

            if (oldTime == 0) //First time ever
            {
                timeText.text = newTime + "s";
            }
            else if (newTime < oldTime) //New highscore
            {
                timeText.text = "<s>" + oldTime + "</s>" + "/" + newTime;
            }
            else //Old time is better
            {
                timeText.text = "<s>" + newTime + "</s>" + "/" + oldTime;
            }         
        }

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
            //Audio
            AudioManager.audioMan.PlayLevelTimeSound();

            //Get rect & scale it bigger
            RectTransform rect = canvasGroup.GetComponent<RectTransform>();
            rect.localScale = new Vector3(1.5f, 1.5f, 1f);

            while (canvasGroup.alpha < 1)
            {
                //Fade in
                canvasGroup.alpha += 3f * Time.deltaTime;

                //Scale back to normal
                rect.localScale = Vector3.MoveTowards(rect.localScale, Vector3.one, 2f * Time.deltaTime);

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
