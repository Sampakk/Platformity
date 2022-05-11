using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioMan;
    AudioSource audioSrc;

    public AudioClip footstepSound;
    public AudioClip throwSound;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip completeSound;

    // Start is called before the first frame update
    void Start()
    {
        if (audioMan == null) audioMan = this;
        else Destroy(gameObject);

        audioSrc = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstepSound(float volume)
    {
        audioSrc.PlayOneShot(footstepSound, volume);
    }

    public void PlayThrowSound()
    {
        audioSrc.PlayOneShot(throwSound);
    }

    public void PlayJumpSound()
    {
        audioSrc.PlayOneShot(jumpSound);
    }

    public void PlayDeathSound()
    {
        audioSrc.PlayOneShot(deathSound);
    }

    public void PlayCompleteSound()
    {
        audioSrc.PlayOneShot(completeSound, 0.2f);
    }
}
