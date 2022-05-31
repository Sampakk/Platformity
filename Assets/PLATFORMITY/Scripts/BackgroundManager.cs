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
        chapter = (int)char.GetNumericValue(sceneName[2]);

        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            //Get background index which starts from 0 so its -1
            background = backgrounds[chapter - 1];

            //Set sprites from the scriptable objects
            backgroundSR.sprite = background.backSprite;
            middlegroundSR.sprite = background.middleSprite;
            foregroundSR.sprite = background.foreSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posB = backgroundSR.transform.position;
        Vector3 posM = middlegroundSR.transform.position;
        Vector3 posF = foregroundSR.transform.position;
        Quaternion rot = backgroundRoot.transform.rotation;

        if (player != null && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1) 
        {
            Vector3 scaleB = new Vector3(-player.transform.position.x * background.parallaxAmountScaleb, -player.transform.position.x * background.parallaxAmountScaleb, 0f);
            Vector3 scaleM = new Vector3(-player.transform.position.x * background.parallaxAmountScalem, -player.transform.position.x * background.parallaxAmountScalem, 0f);
            Vector3 scaleF = new Vector3(-player.transform.position.x * background.parallaxAmountScalef, -player.transform.position.x * background.parallaxAmountScalef, 0f);

            //Transfrom positions and rotation relative to player for back image
            posB.x = -player.transform.position.x * background.parallaxAmountXb;
            posB.y = -player.transform.position.y * background.parallaxAmountYb;
            rot.eulerAngles = new Vector3(0, 0, (player.transform.position.x * background.parallaxAmountZrotb) + (-player.transform.position.y * background.parallaxAmountZrotb));

            backgroundSR.transform.position = posB;
            backgroundSR.transform.rotation = rot;

            if (background.parallaxAmountScaleb != 0) 
            {
                if (-background.parallaxScaleDeadzoneb < player.transform.position.x && player.transform.position.x < background.parallaxScaleDeadzoneb) backgroundSR.transform.localScale = new Vector3(background.parallaxAmountScaleb * background.parallaxScaleDeadzoneb, background.parallaxAmountScaleb * background.parallaxScaleDeadzoneb, 0);
                else backgroundSR.transform.localScale = scaleB;
            }

            //Transfrom positions and rotation relative to player for middle image
            posM.x = -player.transform.position.x * background.parallaxAmountXm;
            posM.y = -player.transform.position.y * background.parallaxAmountYm;
            rot.eulerAngles = new Vector3(0, 0, (player.transform.position.x * background.parallaxAmountZrotm) + (-player.transform.position.y * background.parallaxAmountZrotm));

            middlegroundSR.transform.position = posM;
            middlegroundSR.transform.rotation = rot;

            if (background.parallaxAmountScalem != 0)
            {
                if (-background.parallaxScaleDeadzonem < player.transform.position.x && player.transform.position.x < background.parallaxScaleDeadzonem) middlegroundSR.transform.localScale = new Vector3(background.parallaxAmountScalem * background.parallaxScaleDeadzonem, background.parallaxAmountScalem * background.parallaxScaleDeadzonem, 0);
                else middlegroundSR.transform.localScale = scaleM;
            }

            //Transfrom positions and rotation relative to player for fore image
            posF.x = -player.transform.position.x * background.parallaxAmountXf;
            posF.y = -player.transform.position.y * background.parallaxAmountYf;
            rot.eulerAngles = new Vector3(0, 0, (player.transform.position.x * background.parallaxAmountZrotf) + (-player.transform.position.y * background.parallaxAmountZrotf));

            foregroundSR.transform.position = posF;
            foregroundSR.transform.rotation = rot;

            if (background.parallaxAmountScalef != 0) 
            {
                if (-background.parallaxScaleDeadzonef < player.transform.position.x && player.transform.position.x < background.parallaxScaleDeadzonef) foregroundSR.transform.localScale = new Vector3(background.parallaxAmountScalef * background.parallaxScaleDeadzonef, background.parallaxAmountScalef * background.parallaxScaleDeadzonef, 0);
                else foregroundSR.transform.localScale = scaleF;
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
