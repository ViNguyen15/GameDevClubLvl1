using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDash : MonoBehaviour {

    private Control player;

	// Use this for initialization
	void Start () {
        player = Object.FindObjectOfType<Control>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.enableDashUp();
            Destroy(gameObject);
        }
    }
}
