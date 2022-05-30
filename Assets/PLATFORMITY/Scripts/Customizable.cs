using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customizable", menuName = "Customizable", order = 1)]
public class Customizable : ScriptableObject
{
    public string itemName = "New Item";
    public int itemPrice = 100;
    public Sprite itemSprite;

    public enum ItemType {Hat, Eyes};
    public ItemType itemType;
}
