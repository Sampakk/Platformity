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
        Vector3 pos = backgroundRoot.transform.position;
        Quaternion rot = backgroundRoot.transform.rotation;

        if (player != null && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1) 
        {
            //Transfrom positions and rotation relative to player
            pos.x = -player.transform.position.x * background.parallaxAmountXf;
            pos.y = -player.transform.position.y * background.parallaxAmountYf;
            rot.eulerAngles = new Vector3(0, 0, -player.transform.position.x * background.parallaxAmountZrotf);

            backgroundRoot.transform.position = pos;
            backgroundRoot.transform.rotation = rot;
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
