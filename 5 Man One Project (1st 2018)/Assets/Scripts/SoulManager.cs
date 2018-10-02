using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoulManager {

    public Transform spawnPointSoul;
    [HideInInspector] public GameObject instance;

    private Soul soul;

    // Use this for initialization
    public void Setup()
    {
        soul = instance.GetComponent<Soul>();           //Besoin d'une référence au GO quand il est instancié dans le GameManager
    }


    public void Reset()                 //reset la position de la soul
    {
        instance.transform.position = spawnPointSoul.transform.position;
        instance.transform.rotation = spawnPointSoul.transform.rotation;
        soul.collidedOnce = false;
    }


}
