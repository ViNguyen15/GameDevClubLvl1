using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip jumpSound, walkSound;
    static AudioSource audioSrc;
	// Use this for initialization
	void Start () {

        jumpSound = Resources.Load<AudioClip>("jump");
        walkSound = Resources.Load<AudioClip>("walk");
        audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "walk":
                audioSrc.PlayOneShot(walkSound);
                break;

        }
    }
}
