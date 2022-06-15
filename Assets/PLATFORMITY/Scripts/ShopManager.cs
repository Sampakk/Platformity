using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    [Header("Categories")]
    public GameObject[] categories;

    [Header("Items")]
    public Transform itemsRoot;
    List<Item> hatItems = new List<Item>();

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
            //Enable category & get items in it
            categories[i].SetActive(true);
            hatItems.AddRange(categories[i].GetComponentsInChildren<Item>());

            //Enable first category & disable others
            if (i == category) categories[i].SetActive(true);
            else categories[i].SetActive(false);
        }
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

    public void UpdateOwnedHats()
    {
        string ownedHats = PlayerPrefs.GetString("OwnedHats");

        for (int i = 0; i < ownedHats.Length; i++)
        {
            //Check if owned & update items text
            bool owned = (ownedHats[i] == '1') ? true : false;
            hatItems[i].UpdateTexts(owned);
        }
    }

    public void ChangeCategory(bool next)
    {
        //Clear selected button
        EventSystem.current.SetSelectedGameObject(null);

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
