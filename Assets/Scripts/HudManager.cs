using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        //Update level text
        string levelName = SceneManager.GetActiveScene().name;
        levelText.text = "Level: " + levelName;
    }

    // Update is called once per frame
    void Update()
    {
        //Back to menu with esc
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }
}
