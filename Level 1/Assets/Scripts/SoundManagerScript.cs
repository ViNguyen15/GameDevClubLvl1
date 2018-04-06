using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {
    
    public static AudioClip jumpSound, walkSound,dashSound, teleportSound;
    static AudioSource audioSrc;
    //walking 
    CharacterController cc;
    // Use this for initialization
    
    void Start ()
    {
        walkSound = Resources.Load<AudioClip>("walk(cut)");
        jumpSound = Resources.Load<AudioClip>("jump(cut)");
        dashSound = Resources.Load<AudioClip>("dash(cut)");
        audioSrc = GetComponent<AudioSource>();
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
    
    public static void PlaySound(string clip)
    {

        switch(clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "teleport":
                audioSrc.PlayOneShot(teleportSound);
                break;
            

        }
        if (!audioSrc.isPlaying)
        {
            audioSrc.clip = walkSound;
            audioSrc.Play();
        }
    }
}
