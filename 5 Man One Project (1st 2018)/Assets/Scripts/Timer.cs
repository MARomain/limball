using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    public TextMeshPro timerText;
    public bool timerOn = false;
    public float gameTime = 120f;


	void Update ()
    {
        if (timerOn)        //si le timer est activé
        {
            gameTime -= Time.deltaTime;
        }



        string minutes = Mathf.Floor(gameTime / 60).ToString("00");
        string seconds = (gameTime % 60).ToString("00");

        timerText.text = minutes + ":" + seconds;
	}
}
