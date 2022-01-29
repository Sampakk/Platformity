using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "Timer: " + Time.time.ToString("F1");

        string levelName = SceneManager.GetActiveScene().name;
        levelText.text = "Level: " + levelName;
    }
}
