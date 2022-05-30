using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Backgrounds", order = 1)]
public class Background : ScriptableObject
{
    public int chapter;

    public Sprite backSprite;
    public Sprite middleSprite;
    public Sprite foreSprite;

    public float parallaxAmountXb;
    public float parallaxAmountYb;
    public float parallaxAmountZrotb;

    public float parallaxAmountXm;
    public float parallaxAmountYm;
    public float parallaxAmountZrotm;

    public float parallaxAmountXf;
    public float parallaxAmountYf;
    public float parallaxAmountZrotf;
}
