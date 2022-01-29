using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public RectTransform header;
    public GameObject levelsRoot;

    TextMeshProUGUI headerText;
    float headerFontSize;

    // Start is called before the first frame update
    void Start()
    {
        //Get header text
        headerText = header.GetComponent<TextMeshProUGUI>();
        headerFontSize = headerText.fontSize;

        //Setup level buttons
        Button[] levelButtons = levelsRoot.GetComponentsInChildren<Button>();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            Button button = levelButtons[i];

            //Add onClick function to button
            int levelIndex = i + 1;
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

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
