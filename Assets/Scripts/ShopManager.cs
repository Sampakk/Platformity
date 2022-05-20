using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    int coins;

    public TextMeshProUGUI coinsText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoinsText()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = "Coins: " + coins;
    }
}
