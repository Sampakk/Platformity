using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public Customizable customizable;

    [Header("Texts")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;

    [Header("Hovering")]
    public float hoverRange = 0.5f;
    public float hoverSpeed = 2f;
    float offset;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        //Update texts
        itemNameText.text = customizable.itemName;
        itemPriceText.text = "Price: " + customizable.itemPrice;

        //Update sprite
        SpriteRenderer itemSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        itemSprite.sprite = customizable.itemSprite;
        itemSprite.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * hoverSpeed + offset) * (hoverRange / 2f);
        transform.position = new Vector3(transform.position.x, startPos.y + yOffset, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CustomizationManager customization = FindObjectOfType<CustomizationManager>();
        if (customization != null)
            customization.BuyCustomizable(customizable);
    }

    public void SetOffset(float offset)
    {
        this.offset = offset;
    }
}
