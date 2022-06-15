using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class HudManager : MonoBehaviour
{
    public static HudManager hudMan;

    EventSystem eventSystem;
    TimerManager timer;
   
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI highScoreText;

    [Header("Root Windows")]
    public GameObject hudRoot;
    public GameObject gameplayRoot;

    [Header("Completion")]
    public GameObject completionRoot;
    public TextMeshProUGUI completeText;
    public TextMeshProUGUI earnedCoinsText;
    public float fadeInSpeed = 3f;

    [Header("Buttons")]
    public Button continueButton;
    public Button menuButton;

    [Header("Levels")]
    public GameObject levelTimesRoot;
    public TextMeshProUGUI[] levelTimeTexts;

    // Start is called before the first frame update
    void Start()
    {
        if (hudMan == null) hudMan = this;
        else Destroy(gameObject);

        //Get components
        eventSystem = GetComponentInChildren<EventSystem>();
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

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) //On menu
        {
            if (hudRoot.activeSelf) //Hide hud
                hudRoot.SetActive(false);

            if (gameplayRoot.activeSelf) //Hide gameplay root
                gameplayRoot.SetActive(false);
        }
        else if (sceneIndex == 1) //On shop
        {
            if (!hudRoot.activeSelf) //Show hud
                hudRoot.SetActive(true);

            if (gameplayRoot.activeSelf) //Hide gameplay root
                gameplayRoot.SetActive(false);
        }
        else //On levels
        {
            if (!hudRoot.activeSelf) //Show hud
                hudRoot.SetActive(true);

            if (!gameplayRoot.activeSelf) //Show gameplay root
                gameplayRoot.SetActive(true);
        }

        //Back to menu with esc, only if completion screen is not shown
        if (!completionRoot.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadToMenu();
                timer.StopTimer();
            }
        }  
    }

    public void UpdateCompletion(string chapter, bool firstTime)
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

            timeText.color = Color.white;

            if (oldTime == 0) //First time ever
            {
                timeText.text = newTime + "s";
            }
            else if (newTime < oldTime) //New highscore
            {
                timeText.text = "<s>" + oldTime + "</s>" + "/" + newTime;
                timeText.color = Color.green;
            }
            else //Old time is better
            {
                timeText.text = /*"<s>" + newTime + "</s>" + "/" +*/"" + oldTime;
            }         
        }

        //Hide earned coins & buttons
        earnedCoinsText.text = "";
        continueButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);

        //Start fading in the levels
        StartCoroutine(FadeInLevels(firstTime));
    }

    IEnumerator FadeInLevels(bool firstTime)
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
                canvasGroup.alpha += fadeInSpeed * Time.deltaTime;

                //Scale back to normal
                rect.localScale = Vector3.MoveTowards(rect.localScale, Vector3.one, 2f * Time.deltaTime);

                yield return null;
            }
        }

        //Fade in earned coins & buttons
        StartCoroutine(FadeInCoinsAndButtons(firstTime));
    }

    IEnumerator FadeInCoinsAndButtons(bool firstTime)
    {
        if (firstTime) //Earned coins on first time
        {
            string coinsString = "You earned 50 coins!";

            //Fade in earned coins
            for (int i = 0; i < coinsString.Length; i++)
            {
                earnedCoinsText.text += coinsString[i];

                if (coinsString[i] != ' ') //Skip empty spaces
                    yield return new WaitForSeconds(0.05f);
            }
        }
        
        yield return new WaitForSeconds(0.25f);

        //Enable buttons
        continueButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    public void LoadNextChapter()
    {
        //Reset button scales
        continueButton.GetComponent<ButtonController>().ResetMouseOver();
        menuButton.GetComponent<ButtonController>().ResetMouseOver();

        GameManager.game.LoadNextChapter();
    }

    public void LoadToMenu()
    {
        //Reset button scales
        continueButton.GetComponent<ButtonController>().ResetMouseOver();
        menuButton.GetComponent<ButtonController>().ResetMouseOver();

        //Change to menu music
        AudioManager.audioMan.ChangeMusic(true);

        SceneManager.LoadScene(0);
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
        //Clear selected button
        eventSystem.SetSelectedGameObject(null);

        completionRoot.SetActive(false);

        //Hide or show timer
        if (PlayerPrefs.GetInt("Timer") == 0) //Hide
        {
            timerText.enabled = false;
            highScoreText.enabled = false;
        }
        else //Show
        {
            timerText.enabled = true;
            highScoreText.enabled = true;
        }
    }
}
