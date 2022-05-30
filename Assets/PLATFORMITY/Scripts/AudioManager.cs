using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioMan;
    AudioSource audioSrc;
    AudioSource musicSrc;

    [Header("UI Sounds")]
    public AudioClip levelTimeSound;
    public AudioClip buySound;

    [Header("Player Sounds")]
    public AudioClip throwSound;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip completeSound;

    [Header("Footsteps")]
    public AudioClip[] footstepSounds;
    int footstepIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (audioMan == null) audioMan = this;
        else Destroy(gameObject);

        //Get components
        audioSrc = GetComponent<AudioSource>();
        musicSrc = transform.GetChild(0).GetComponent<AudioSource>();

        //Setup music volume
        UpdateMusicVolume();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMusicVolume()
    {
        musicSrc.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void PlayFootstepSound(float volume)
    {
        if (footstepIndex == 0) footstepIndex++;
        else footstepIndex = 0;

        audioSrc.PlayOneShot(footstepSounds[footstepIndex], volume);
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

    public void PlayLevelTimeSound()
    {
        audioSrc.PlayOneShot(levelTimeSound, 1f);
    }

    public void PlayBuySound()
    {
        audioSrc.PlayOneShot(buySound, 1f);
    }
}
