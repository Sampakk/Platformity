using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    Canvas canvas;

    [Header("Roots")]
    public GameObject levelsRoot;
    public GameObject controlsRoot;

    [Header("Menu & Levels")]
    public RectTransform header;
    public Scrollbar levelScrollbar;
    public GameObject levelButtonRoots;

    [Header("Gamemode Buttons")]
    public Button normalModeButton;
    public Button hardModeButton;
    public Button hcModeButton;
    string normalModeText;
    string hardModeText;
    string hcModeText;

    [Header("Timer Toggle")]
    public Toggle timerToggle;

    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeText;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeText;

    [Header("Black Screen")]
    public Image blackScreen;
    public float fadeTime = 1f;

    [Header("Dynamic Menu")]
    public RectTransform levelsTurnable;
    public RectTransform controlsTurnable;
    public float threshold = 0.55f;
    public float maxAngle = 5f;
    public float turnSpeed = 4f;

    Button[] levelButtons;
    TextMeshProUGUI headerText;
    float headerFontSize;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();

        //Show cursor & unlock
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Hide controls root
        ShowLevels();

        //Get header text
        headerText = header.GetComponent<TextMeshProUGUI>();
        headerFontSize = headerText.fontSize;

        //Setup timer toggle
        if (PlayerPrefs.GetInt("Timer", 0) == 0) timerToggle.isOn = false;
        else timerToggle.isOn = true;

        //Setup master volume slider
        masterVolumeSlider.onValueChanged.AddListener(delegate { UpdateMasterVolume(); });
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        masterVolumeText.text = "Master volume: " + masterVolumeSlider.value;

        AudioListener.volume = masterVolumeSlider.value;

        //Setup music volume slider
        musicVolumeSlider.onValueChanged.AddListener(delegate { UpdateMusicVolume(); });
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        musicVolumeText.text = "Music Volume: " + musicVolumeSlider.value;

        //Setup scrollbar to be top position
        levelScrollbar.value = 1f;

        //Setup level buttons
        levelButtons = levelButtonRoots.GetComponentsInChildren<Button>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];
            button.gameObject.AddComponent<ButtonController>();

            //Setup highlight color to button
            ColorBlock colors = button.colors;
            colors.highlightedColor = Color.green;
            button.colors = colors;

            //Add onClick function to button
            int levelIndex = i + 2;
            button.onClick.AddListener(delegate { LoadLevel(levelIndex); });

            //Check if level isn't completed and then disable button
            if (levelIndex > 2)
            {
                string levelPrefsName = "Level" + levelIndex;

                if (PlayerPrefs.GetInt(levelPrefsName) == 0)
                    button.interactable = false;
            }
        }

        //Setup gamemode buttons on start
        normalModeButton.interactable = true;
        hardModeButton.interactable = false;
        hcModeButton.interactable = false;
        StartCoroutine(EnableGamemodeButtons());

        UpdateGameMode(PlayerPrefs.GetInt("Gamemode", 0));
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHeader();

        AnimateContent();
    }

    void AnimateHeader()
    {
        //Rotate header
        Vector3 eulers = header.rotation.eulerAngles;
        eulers.z = Mathf.Sin(Time.time * 2f) * 5f;

        header.rotation = Quaternion.Euler(eulers);

        //Scale font
        headerText.fontSize = headerFontSize + Mathf.Sin(Time.time * 4f) * 5f;
    }

    void AnimateContent()
    {
        //Get mouse position from center of the screen
        Vector2 mousePos = Input.mousePosition;
        mousePos.x = mousePos.x - Screen.width / 2;
        mousePos.y = mousePos.y - Screen.height / 2;

        //Get multiplier from mouse position
        Vector2 mouseMultiplier = new Vector2(mousePos.x / (Screen.width / 2), mousePos.y / (Screen.height / 2));
        mouseMultiplier.x = Mathf.Clamp(mouseMultiplier.x, -1f, 1f);
        mouseMultiplier.y = Mathf.Clamp(mouseMultiplier.y, -1f, 1f);

        bool canRotateX = (mouseMultiplier.x > -threshold && mouseMultiplier.x < threshold) ? false : true;
        bool canRotateY = (mouseMultiplier.y > -threshold && mouseMultiplier.y < threshold) ? false : true;

        if (!canRotateX && !canRotateY)
        {
            mouseMultiplier.x = 0;
            mouseMultiplier.y = 0;
        }        

        //Create rotation towards mouse
        Vector3 rotation = new Vector3(-mouseMultiplier.y * maxAngle, mouseMultiplier.x * maxAngle, 0f);

        if (levelsRoot.activeSelf) //Rotate levels
            levelsTurnable.localRotation = Quaternion.Slerp(levelsTurnable.localRotation, Quaternion.Euler(rotation), turnSpeed * Time.deltaTime);
        
        if (controlsRoot.activeSelf) //Rotate controls
            controlsTurnable.localRotation = Quaternion.Slerp(controlsTurnable.localRotation, Quaternion.Euler(rotation), turnSpeed * Time.deltaTime);
    }

    //Saves master volume when slider is moved
    public void UpdateMasterVolume()
    {
        float value = (float)System.Math.Round(masterVolumeSlider.value, 1);
        masterVolumeSlider.value = value;
        masterVolumeText.text = "Master volume: " + value;

        PlayerPrefs.SetFloat("MasterVolume", value);
        AudioListener.volume = value;
    }

    //Saves music volume when slider is moved
    public void UpdateMusicVolume()
    {
        float value = (float)System.Math.Round(musicVolumeSlider.value, 1);
        musicVolumeSlider.value = value;
        musicVolumeText.text = "Music volume: " + value;

        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioManager.audioMan.UpdateMusicVolume();
    }

    //Updates gamemode to playerprefs
    public void UpdateGameMode(int index)
    {
        Button[] gamemodeButtons = { normalModeButton, hardModeButton, hcModeButton };
        string[] gamemodeTexts = { "Normal", "Hard", "HC" };

        //Update buttons visuals
        for (int i = 0; i < gamemodeButtons.Length; i++)
        {
            if (index == i) //Selected button
            {
                TextMeshProUGUI text = gamemodeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = "> " + gamemodeTexts[i] + " <";
            }
            else //Other button
            {
                TextMeshProUGUI text = gamemodeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = gamemodeTexts[i];
            }
        }

        //After that, load new mode that we want
        PlayerPrefs.SetInt("Gamemode", index);
        UpdateLevelButtons();

        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);
    }

    IEnumerator EnableGamemodeButtons()
    {
        //Wait for steam manager to be initialized
        while (!SteamAchievements.achievements.initialized)
            yield return null;

        if (SteamAchievements.achievements.HasCompletedAchievement("ACH_NORMAL_COMPLETED")) //Completed normal
        {
            hardModeButton.interactable = true;
        }

        if (SteamAchievements.achievements.HasCompletedAchievement("ACH_HARD_COMPLETED")) //Completed hard
        {
            hcModeButton.interactable = true;
        }
    }

    void UpdateLevelButtons()
    {
        int gamemode = PlayerPrefs.GetInt("Gamemode", 0);
        Debug.Log("Gamemode: " + gamemode);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];
            int levelIndex = i + 2;

            //Check if level isn't completed and then disable button
            if (levelIndex > 2)
            {
                string levelPrefsName = "Level" + levelIndex;
                string hardLevelPrefsName = "HardLevel" + levelIndex;

                if (gamemode == 0) //Normal mode
                {
                    if (PlayerPrefs.GetInt(levelPrefsName, 0) == 0)
                    {
                        button.interactable = false;
                    }
                    else //Unlocked, check if not the first level of chapter
                    {
                        button.interactable = true;
                    }
                }
                else if (gamemode == 1) //Hard mode
                {
                    if ((levelIndex - 2) % 5 != 0)
                    {
                        button.interactable = false;
                    }
                    else
                    {
                        if (PlayerPrefs.GetInt(hardLevelPrefsName, 0) == 1)
                        {
                            button.interactable = true;
                        }
                        else
                        {
                            button.interactable = false;
                        }
                    }                      
                }
                else if (gamemode == 2) //HC mode
                {
                    button.interactable = false;
                }
            }
        }
    }

    //Updates timer toggle to playerprefs
    public void UpdateTimerToggle()
    {
        int state = (timerToggle.isOn) ? 1 : 0;
        PlayerPrefs.SetInt("Timer", state);
    }

    //Show levels root
    public void ShowLevels()
    {
        levelsRoot.SetActive(true);
        controlsRoot.SetActive(false);

        StartCoroutine(FadeInContent(true));

        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);
    }

    //Show controls root
    public void ShowControls()
    {
        levelsRoot.SetActive(false);
        controlsRoot.SetActive(true);

        StartCoroutine(FadeInContent(false));

        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);
    }

    IEnumerator FadeInContent(bool isLevels)
    {
        //Get correct canvas group
        CanvasGroup canvasGroup = null;
        if (isLevels) canvasGroup = levelsTurnable.GetComponent<CanvasGroup>();
        else canvasGroup = controlsTurnable.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 2f;

            yield return null;
        }
    }

    //Load to shop scene
    public void LoadShop()
    {
        StartCoroutine(FadeInAndLoadLevel(1));
    }

    //Load to level with index
    public void LoadLevel(int index)
    {
        StartCoroutine(FadeInAndLoadLevel(index));
    }

    //Reset all playerprefs
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();

        //Reload scene
        SceneManager.LoadScene(0);
    }

    //Exit game
    public void ExitGame()
    {
        Application.Quit();
    }

    //Fade in to black & change scene
    IEnumerator FadeInAndLoadLevel(int index)
    {
        //Black screen will block all raycasts
        blackScreen.raycastTarget = true;

        //Load level sound
        UISounds.sounds.PlayLoadLevelSound();

        //Wait for to fade in
        while (blackScreen.color.a < 1f)
        {
            Color color = blackScreen.color;
            color.a = Mathf.MoveTowards(color.a, 1f, 1f / fadeTime * Time.deltaTime);
            blackScreen.color = color;

            yield return null;
        }

        //Load level
        SceneManager.LoadScene(index);
    }
}
