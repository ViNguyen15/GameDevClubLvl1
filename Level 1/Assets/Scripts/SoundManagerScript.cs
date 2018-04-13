using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip jumpSound, walkSound, dashSound, teleportSound;
    static AudioSource audioSrc;
    //walking 
    public Control mPlayer;
    public bool grounded;

    // Use this for initialization
    void Start()
    {
        walkSound = Resources.Load<AudioClip>("walk(cut)");
        jumpSound = Resources.Load<AudioClip>("jump");
        dashSound = Resources.Load<AudioClip>("dash(cut)");
        teleportSound = Resources.Load<AudioClip>("teleport(cut)");
        audioSrc = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string clip)
    {

        switch (clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "teleport":
                audioSrc.PlayOneShot(teleportSound);
                break;
            case "dash":
                audioSrc.PlayOneShot(dashSound);
                break;


        }
        if (!audioSrc.isPlaying)
        {
            audioSrc.clip = walkSound;
            audioSrc.Play();
        }
    }

    /*
    public void PlayAudio(string clips)
    {

        if (grounded)
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.clip = walkSound;
                audioSrc.Play();
            }
        }
        else
        {
            audioSrc.Stop();
        }
    }
    */

}
