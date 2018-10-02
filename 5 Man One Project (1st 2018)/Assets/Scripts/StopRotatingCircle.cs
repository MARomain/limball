using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotatingCircle : MonoBehaviour {
    Vector3 angle;

    // Use this for initialization
    void Start () {
        angle = transform.rotation.eulerAngles;             //prend la rotation de base
    }
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(angle);       // la maintient tout le temps(empêche le cercle sous le joueur de tourner)
	}
}
