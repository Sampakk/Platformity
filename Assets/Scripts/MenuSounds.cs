using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    AudioSource audioSrc;

    public AudioClip hoverSound;

    // Start is called before the first frame update
    void Start()
    {
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
}
