using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    Animator anim;
    AudioSource audioSrc;
    PlayerMovement player;

    public AudioClip buySound;

    [Header("Customizables")]
    public Customizable[] hats;
    public Customizable[] eyes;

    [Header("Hat")]
    public Transform hatRoot;
    public SpriteRenderer hatSprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        player = GetComponentInParent<PlayerMovement>();

        UpdateCustomizationsOnStart();
    }

    // Update is called once per frame
    void Update()
    {
        Animations();
    }

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

    void UpdateCustomizationsOnStart()
    {
        //Update hat
        int equippedHat = PlayerPrefs.GetInt("Hat", 0);
        EquipCustomizable(hats[equippedHat]);
    }

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

    public void BuyCustomizable(Customizable customizable)
    {
        //Get our coins
        int coins = PlayerPrefs.GetInt("Coins", 0);

        if (customizable.itemType == Customizable.ItemType.Hat) //Hat
        {
            //Get indx of hat we are going to buy
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
            string hatsString = "";
            for (int i = 0; i < hats.Length; i++)
            {
                if (i == 0)
                {
                    hatsString += "1";
                }
                else
                {
                    if (hats[i] == customizable)
                        hatsString += "1";
                    else
                        hatsString += "0";
                }
            }

            int[] hatsArray = hatsString.Select(c => int.Parse(c.ToString())).ToArray();

            //Get owned hats from playerprefs & convert to array
            string ownedHats = PlayerPrefs.GetString("OwnedHats", hatsString);
            int[] ownedHatsArray = ownedHats.Select(c => int.Parse(c.ToString())).ToArray();

            //Update prefs string
            for (int i = 0; i < hats.Length; i++)
            {
                int owned = ownedHatsArray[i];
                int unlocked = hatsArray[i];

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
                            audioSrc.PlayOneShot(buySound, 1f);

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
        }
        else if (customizable.itemType == Customizable.ItemType.Eyes) //Eyes
        {

        }
    }
}
