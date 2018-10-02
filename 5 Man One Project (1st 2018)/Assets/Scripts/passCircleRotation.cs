using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passCircleRotation : MonoBehaviour {

    private Soul soul;
    public GameObject soulGO;
    public float turnSpeed;
    private GameManager gameManager;

	// Use this for initialization
	void Start ()
    {
        soul = soulGO.GetComponentInParent<Soul>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (soul.playerMovement != Vector3.zero)
        {
            for (int i = 0; i < gameManager.players.Length; i++)
            {
                if (gameManager.players[i].playerController.hasTheSoul == true)
                {
                    transform.rotation = Quaternion.Slerp(gameManager.players[i].instance.transform.rotation, Quaternion.LookRotation(gameManager.players[i].playerController.movement), turnSpeed);
                }
            }
        }
    }
}