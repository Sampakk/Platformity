using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    public static MenuSounds sounds;
    AudioSource audioSrc;

    public AudioClip hoverSound;
    public AudioClip loadLevelSound;

    // Start is called before the first frame update
    void Start()
    {
        if (sounds == null)
            sounds = this;

        audioSrc = GetComponent<AudioSource>();
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
