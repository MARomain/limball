using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalAngel : MonoBehaviour {

    public int numGoalAngel;
    public TextMeshPro demon3Dtext;
    public bool scoredOnAngelGoal;
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Soul soul = other.GetComponent<Soul>();
        PlayerController player = other.GetComponent<PlayerController>();
        if (soul)
        {
                numGoalAngel += 1;                          //ajoute 1 au score
                demon3Dtext.text = numGoalAngel.ToString(); //update le text du score
                                                            //player.hasTheSoul = false;
                Destroy(other.gameObject);  // fait disparaitre la soul quand un but est marqué
                scoredOnAngelGoal = true;
            particles.Play();
        }



        // *** Méthode avec tag moins propre i think ***
        //if (other.gameObject.tag == "soul")
        //{
        //    Debug.Log("soul");
        //    Destroy(other.gameObject);
        //}
    }
}
