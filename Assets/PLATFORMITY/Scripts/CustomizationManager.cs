using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    Animator anim;
    PlayerMovement player;

    [Header("Customizables")]
    public Customizable[] hats;
    public Customizable[] eyes;

    [Header("Hat")]
    public Transform hatRoot;
    public SpriteRenderer hatSprite;

    string ownedHats;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        player = GetComponentInParent<PlayerMovement>();

        SetupOwnedHats();

        UpdateCustomizationsOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        Animations();
    }

    //Animations
    void Animations()
    {
        float moveX = player.GetMoveX();

        //Left and right turning
        float maxAngle = 5f;

        Vector3 localEulers = transform.localEulerAngles;
        localEulers.z = -(moveX * maxAngle);
        transform.localRotation = Quaternion.Euler(localEulers);

        //Move animation
        bool isMoving = (player.IsGrounded() && moveX != 0) ? true : false;
        anim.SetBool("Moving", isMoving);

        //Fall animation/grounded
        anim.SetBool("Grounded", player.IsGrounded());
    }

    //Equips current customizable on start
    void UpdateCustomizationsOnStart()
    {
        //Update hat
        int equippedHat = PlayerPrefs.GetInt("Hat", 0);
        EquipCustomizable(hats[equippedHat]);
    }

    //Setups owned hats string from playerprefs & if it's not correct repairs it on runtime
    void SetupOwnedHats()
    {
        //Setup owned hats string
        string newOwnedHats = "";
        ownedHats = PlayerPrefs.GetString("OwnedHats");
        int[] ownedHatsArray = ownedHats.Select(c => int.Parse(c.ToString())).ToArray();

        for (int i = 0; i < hats.Length; i++)
        {
            if (i == 0) //First hat "no hat" is always owned
            {
                newOwnedHats += "1";
            }
            else
            {
                if (i < ownedHatsArray.Length) //Hat is found already from playerprefs
                {
                    newOwnedHats += ownedHatsArray[i].ToString();
                }
                else //Not found from playerprefs, therefore not owned
                {
                    newOwnedHats += "0";
                }
            }
        }

        //Save initialized hats string
        ownedHats = newOwnedHats;
        PlayerPrefs.SetString("OwnedHats", ownedHats);

        //Update shop items to show what you own
        ShopManager shop = FindObjectOfType<ShopManager>();
        if (shop != null) shop.UpdateOwnedHats();

        Debug.Log("Owned hats: " + ownedHats);
    }

    //Equips the customizable, nothing else
    public void EquipCustomizable(Customizable customizable)
    {
        if (customizable.itemType == Customizable.ItemType.Hat) //Hat
        {
            if (customizable.itemSprite != null)
                hatSprite.sprite = customizable.itemSprite;
            else
                hatSprite.sprite = null;
        }
        else if (customizable.itemType == Customizable.ItemType.Eyes) //Eyes
        {

        }
    }

    //Tries to buy customizable & if player owns it, equips that
    public void BuyCustomizable(Customizable customizable)
    {
        //Get our coins
        int coins = PlayerPrefs.GetInt("Coins", 0);

        if (customizable.itemType == Customizable.ItemType.Hat) //Hat
        {
            //Get index of hat we are going to buy
            int hatIndex = 0;
            for (int i = 0; i < hats.Length; i++)
            {
                if (hats[i] == customizable)
                {
                    hatIndex = i;
                    break;
                }
            }

            //Setup owned hats & convert to array
            string boughtHatString = "";
            for (int i = 0; i < hats.Length; i++)
            {
                if (hats[i] == customizable) boughtHatString += "1";
                else boughtHatString += "0";
            }

            int[] boughtHatArray = boughtHatString.Select(c => int.Parse(c.ToString())).ToArray();

            //Convert owned hats string to array
            int[] ownedHatsArray = ownedHats.Select(c => int.Parse(c.ToString())).ToArray();

            //Update prefs string
            for (int i = 0; i < hats.Length; i++)
            {
                int owned = ownedHatsArray[i];
                int unlocked = boughtHatArray[i];

                //Check if we own the hat
                if (i == hatIndex && owned == 1)
                {
                    //Save our equipped hat to playerprefs
                    PlayerPrefs.SetInt("Hat", i);

                    //Equip hat
                    EquipCustomizable(customizable);
                }
                else //We don't own the hat
                {
                    if (unlocked > owned)
                    {
                        //Check if we have enough coins
                        if (coins >= customizable.itemPrice)
                        {
                            //Audio
                            AudioManager.audioMan.PlayBuySound();

                            //Decrease coins & update shop ui
                            coins -= customizable.itemPrice;
                            PlayerPrefs.SetInt("Coins", coins);
                            FindObjectOfType<ShopManager>().UpdateCoinsText();

                            //Update the one we just bought
                            ownedHatsArray[i] = unlocked;

                            //Save our equipped hat to playerprefs
                            PlayerPrefs.SetInt("Hat", i);

                            //Equip hat
                            EquipCustomizable(customizable);

                            break;
                        }
                    }
                }
            }

            //Save new hat to playerprefs
            ownedHats = string.Join("", ownedHatsArray); //Int array to string
            PlayerPrefs.SetString("OwnedHats", ownedHats);

            //Update shop items to show what you own
            ShopManager shop = FindObjectOfType<ShopManager>();
            if (shop != null) shop.UpdateOwnedHats();

            Debug.Log("Owned hats: " + ownedHats);
        }
        else if (customizable.itemType == Customizable.ItemType.Eyes) //Eyes
        {

        }
    }
}
