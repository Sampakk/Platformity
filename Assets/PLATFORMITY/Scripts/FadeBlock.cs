using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlock : MonoBehaviour
{
    Collider2D col;
    SpriteRenderer sprite;   

    public float fadeTime = 3f;
    bool isFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut()
    {
        if (!isFadeOut)
        {
            isFadeOut = true;

            //Deactivate
            sprite.enabled = false;
            col.enabled = false;

            StartCoroutine(FadeIn(fadeTime));
        } 
    }

    IEnumerator FadeIn(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Activate
        sprite.enabled = true;
        col.enabled = true;

        isFadeOut = false;
    }
}
