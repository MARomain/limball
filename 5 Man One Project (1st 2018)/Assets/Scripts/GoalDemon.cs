using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalDemon : MonoBehaviour {

    public int numGoalDemon;
    public TextMeshPro angel3Dtext;
    public bool scoredOnDemonGoal;
    private ParticleSystem particles;

	// Use this for initialization
	void Start ()
    {
        particles = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Soul soul = other.GetComponent<Soul>();
        
        if (soul)   
        {
            numGoalDemon += 1;
            angel3Dtext.text = numGoalDemon.ToString();
            //player.hasTheSoul = false;              // a la base je voulais le mettre dans la fonction Reset() de SoulManager mais ça fonctionne pas ????
            Destroy(other.gameObject);
            scoredOnDemonGoal = true;
            particles.Play();
        }

        //if(other.gameObject.tag == "Player")
        //{
        //    PlayerController player = other.GetComponent<PlayerController>();
        //    player.hasTheSoul = false;
        //}


        // *** Méthode avec tag moins propre i think ***
        //if (other.gameObject.tag == "soul")
        //{
        //    Debug.Log("soul");
        //    Destroy(other.gameObject);
        //}
    }
}
