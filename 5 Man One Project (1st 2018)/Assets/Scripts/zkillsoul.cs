using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zkillsoul : MonoBehaviour {

    private GameManager gameManager;
    private Soul soul;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        soul = other.GetComponent<Soul>();
        if (soul)
        {
            soul.gameObject.SetActive(false);
            soul.gameObject.transform.position = gameManager.soul.spawnPointSoul.transform.position;
            soul.gameObject.transform.rotation = gameManager.soul.spawnPointSoul.transform.rotation;
            soul.collidedOnce = false;
            soul.executeSeulementApresAvoirCollideSALE = false;
            soul.gameObject.SetActive(true);
            soul.transform.GetChild(0).gameObject.SetActive(false);
            soul.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0f);
            soul.transform.GetChild(1).gameObject.SetActive(false);
            soul.transform.GetChild(2).gameObject.SetActive(false);

        }
    }
}
