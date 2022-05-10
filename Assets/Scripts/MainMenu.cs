using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Menu & Levels")]
    public RectTransform header;
    public GameObject levelsRoot;

    [Header("Master Volume")]
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumeText;

    [Header("Black Screen")]
    public Image blackScreen;
    public float fadeTime = 1f;

    TextMeshProUGUI headerText;
    float headerFontSize;

    // Start is called before the first frame update
    void Start()
    {
        //Show cursor & unlock
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Get header text
        headerText = header.GetComponent<TextMeshProUGUI>();
        headerFontSize = headerText.fontSize;

        //Setup master volume slider
        masterVolumeSlider.onValueChanged.AddListener(delegate { UpdateMasterVolume(); });
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        masterVolumeText.text = "Master volume: " + masterVolumeSlider.value;

        AudioListener.volume = masterVolumeSlider.value;

        //Setup level buttons
        Button[] levelButtons = levelsRoot.GetComponentsInChildren<Button>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];
            button.gameObject.AddComponent<ButtonController>();

            //Add onClick function to button
            int levelIndex = i + 2;
            button.onClick.AddListener(delegate { LoadLevel(levelIndex); });

            //Check if level isn't completed and then disable button
            if (levelIndex > 1)
            {
                string levelPrefsName = "Level" + levelIndex;
                if (PlayerPrefs.GetInt(levelPrefsName) == 0)
                    button.interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate header
        Vector3 eulers = header.rotation.eulerAngles;
        eulers.z = Mathf.Sin(Time.time * 2f) * 5f;

        header.rotation = Quaternion.Euler(eulers);

        //Scale font
        headerText.fontSize = headerFontSize + Mathf.Sin(Time.time * 4f) * 5f;
    }

    public void UpdateMasterVolume()
    {
        float value = (float)System.Math.Round(masterVolumeSlider.value, 1);
        masterVolumeSlider.value = value;
        masterVolumeText.text = "Master volume: " + value;

        PlayerPrefs.SetFloat("MasterVolume", value);
        AudioListener.volume = value;
    }

    public void LoadShop()
    {
        StartCoroutine(FadeInAndLoadLevel(1));
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(FadeInAndLoadLevel(index));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeInAndLoadLevel(int index)
    {
        //Black screen will block all raycasts
        blackScreen.raycastTarget = true;

        //Load level sound
        MenuSounds.sounds.PlayLoadLevelSound();

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
