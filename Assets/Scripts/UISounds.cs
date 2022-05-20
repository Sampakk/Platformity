using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public static UISounds sounds;
    AudioSource audioSrc;

    public AudioClip hoverSound;
    public AudioClip loadLevelSound;

    // Start is called before the first frame update
    void Start()
    {
        if (sounds == null) sounds = this;
        else Destroy(gameObject);

        //Get audiosource
        audioSrc = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHoverSound()
    {
        audioSrc.PlayOneShot(hoverSound, 1f);
    }

    public void PlayLoadLevelSound()
    {
        audioSrc.PlayOneShot(loadLevelSound, 1f);
    }
}
