using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour {

    public Animator animator;
    private PlayerController playerHit;
    private int nbOfPlayerInCollider;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(nbOfPlayerInCollider >= 1)
        {
            animator.SetBool("OpenDoor", true);
        }
        else
        {
            animator.SetBool("OpenDoor", false);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        playerHit = other.GetComponent<PlayerController>();        //attrapage de la référence du joueur
        if (playerHit)
        {
            if(playerHit)
            {
                nbOfPlayerInCollider += 1;
            }
            //animator.Play("DoorAngel_gauche");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        playerHit = other.GetComponent<PlayerController>();        //attrapage de la référence du joueur
        if (playerHit)
        {
            if(playerHit)
            {
                nbOfPlayerInCollider -= 1;
            }
            //animator.Play("Close");
        }

        
    }
}
