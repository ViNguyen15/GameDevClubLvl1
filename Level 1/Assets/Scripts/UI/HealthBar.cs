using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image currentHealthBar;
    private Control player;

    // Use this for initialization
    void Start () {
        player = Object.FindObjectOfType<Control>();

    }

    // Update is called once per frame
    void Update () {
        UpdateHP();
    }

    private void UpdateHP()
    {
        float ratio = player.getHealth() / player.getStartingHealth();
        currentHealthBar.rectTransform.localScale = new Vector3(1, ratio, 1);
    }


}
