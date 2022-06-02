using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Backgrounds", order = 1)]
public class Background : ScriptableObject
{
    public int chapter;

    [Header("Back")]
    public Sprite backSprite;
    public float startScaleXb;
    public float startScaleYb;
    [Header("Middle")]
    public Sprite middleSprite;
    public float startScaleXm;
    public float startScaleYm;

    [Header("Fore")]
    public Sprite foreSprite;
    public float startScaleXf;
    public float startScaleYf;

    [Header("Back values")]
    public float parallaxAmountXb;
    public float parallaxAmountYb;
    public float parallaxAmountZrotb;
    public float parallaxAmountScaleb;
    public float divideAmountb;

    [Header("Middle values")]
    public float parallaxAmountXm;
    public float parallaxAmountYm;
    public float parallaxAmountZrotm;
    public float parallaxAmountScalem;
    public float divideAmountm;

    [Header("Fore values")]
    public float parallaxAmountXf;
    public float parallaxAmountYf;
    public float parallaxAmountZrotf;
    public float parallaxAmountScalef;
    public float divideAmountf;
}
