using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    [Header("Categories")]
    public GameObject[] categories;

    [Header("Items")]
    public Transform itemsRoot;

    int coins;
    int category;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();

        OffsetItems();

        //Setup categories
        for (int i = 0; i < categories.Length; i++)
        {
            if (i == category) categories[i].SetActive(true);
            else categories[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ChangeCategory(true);
        else if (Input.GetKeyDown(KeyCode.Q))
            ChangeCategory(false);
    }

    public void UpdateCoinsText()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = "Coins: " + coins;
    }

    public void ChangeCategory(bool next)
    {
        //Loop to next category
        if (next)
        {
            if (category < categories.Length - 1) category++;
            else category = 0;
        }
        else
        {
            if (category > 0) category--;
            else category = categories.Length - 1;
        }

        //Enable new category
        for (int i = 0; i < categories.Length; i++) 
        {
            if (i == category) categories[i].SetActive(true);
            else categories[i].SetActive(false);
        }

        OffsetItems();
    }

    void OffsetItems()
    {
        //Get all items
        Item[] items = itemsRoot.GetComponentsInChildren<Item>();

        //Set offset for every item
        for (int i = 0; i < items.Length; i++)
        {
            Item item = items[i];
            item.SetOffset(item.hoverRange * i);
        }
    }
}
