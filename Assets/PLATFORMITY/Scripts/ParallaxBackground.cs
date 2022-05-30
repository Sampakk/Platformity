using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    PlayerMovement player;

    public float parallaxAmountX = 0.2f;
    public float parallaxAmountY = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPlayer());   
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 pos = transform.position;
            pos.x = -player.transform.position.x * parallaxAmountX;
            pos.y = -player.transform.position.y * parallaxAmountY;

            transform.position = pos;
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
