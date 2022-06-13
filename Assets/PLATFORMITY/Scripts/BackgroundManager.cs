using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{
    PlayerMovement player;

    public Background[] backgrounds;
    Background background;

    public SpriteRenderer backgroundSR;
    public SpriteRenderer middlegroundSR;
    public SpriteRenderer foregroundSR;
    public GameObject backgroundRoot;

    int chapter;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(GetPlayer());

        //Get scenename and extract chapter number from it
        string sceneName = SceneManager.GetActiveScene().name;
        string chapter = "";
        for (int i = 0; i < sceneName.Length; i++) if (i > 1) chapter += sceneName[i];

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            //Get background index which starts from 0 so its -1
            background = backgrounds[int.Parse(chapter) - 1];
            Debug.Log("Background Chapter: " + chapter + " - 1");

            //Set sprites from the scriptable objects
            backgroundSR.sprite = background.backSprite;
            middlegroundSR.sprite = background.middleSprite;
            foregroundSR.sprite = background.foreSprite;
        }

        backgroundSR.transform.localScale = new Vector3(background.startScaleXb, background.startScaleYb, 1);
        middlegroundSR.transform.localScale = new Vector3(background.startScaleXm, background.startScaleYm, 1);
        foregroundSR.transform.localScale = new Vector3(background.startScaleXf, background.startScaleYf, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posB = backgroundSR.transform.position;
        Vector3 posM = middlegroundSR.transform.position;
        Vector3 posF = foregroundSR.transform.position;
        Quaternion rot = backgroundRoot.transform.rotation;

        if (player != null && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1 && background != null) 
        {
            Vector3 scaleB = new Vector3((Mathf.Abs(-player.transform.position.x / background.divideAmountb) - 1 * -background.parallaxAmountScaleb), (Mathf.Abs(-player.transform.position.x / background.divideAmountb) - 1 * -background.parallaxAmountScaleb), 0f);
            Vector3 scaleM = new Vector3((Mathf.Abs(-player.transform.position.x / background.divideAmountm) -1 * -background.parallaxAmountScalem) , (Mathf.Abs(-player.transform.position.x / background.divideAmountm) - 1 * -background.parallaxAmountScalem), 0f);
            Vector3 scaleF = new Vector3((Mathf.Abs(-player.transform.position.x / background.divideAmountf) - 1 * -background.parallaxAmountScalef), (Mathf.Abs(-player.transform.position.x / background.divideAmountf) - 1 * -background.parallaxAmountScalef), 0f);

            //Transfrom positions and rotation relative to player for back image
            posB.x = -player.transform.position.x * background.parallaxAmountXb;
            posB.y = -player.transform.position.y * background.parallaxAmountYb;
            rot.eulerAngles = new Vector3(0, 0, (player.transform.position.x * background.parallaxAmountZrotb) + (-player.transform.position.y * background.parallaxAmountZrotb));

            backgroundSR.transform.position = posB;
            backgroundSR.transform.rotation = rot;

            if (background.parallaxAmountScaleb != 0) 
            {
                backgroundSR.transform.localScale = scaleB;
            }

            //Transfrom positions and rotation relative to player for middle image
            posM.x = -player.transform.position.x * background.parallaxAmountXm;
            posM.y = -player.transform.position.y * background.parallaxAmountYm;
            rot.eulerAngles = new Vector3(0, 0, (player.transform.position.x * background.parallaxAmountZrotm) + (-player.transform.position.y * background.parallaxAmountZrotm));

            middlegroundSR.transform.position = posM;
            middlegroundSR.transform.rotation = rot;

            if (background.parallaxAmountScalem != 0)
            {
                middlegroundSR.transform.localScale = scaleM;
            }

            //Transfrom positions and rotation relative to player for fore image
            posF.x = -player.transform.position.x * background.parallaxAmountXf;
            posF.y = -player.transform.position.y * background.parallaxAmountYf;
            rot.eulerAngles = new Vector3(0, 0, (player.transform.position.x * background.parallaxAmountZrotf) + (-player.transform.position.y * background.parallaxAmountZrotf));

            foregroundSR.transform.position = posF;
            foregroundSR.transform.rotation = rot;

            if (background.parallaxAmountScalef != 0) 
            {
                foregroundSR.transform.localScale = scaleF;
            }
        }
    }

    IEnumerator GetPlayer()
    {
        while (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();

            yield return null;
        }
    }
}
