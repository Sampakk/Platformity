using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    int coins;

    public TextMeshProUGUI coinsText;

    [Header("Items")]
    public Transform itemsRoot;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinsText();

        OffsetItems();
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
