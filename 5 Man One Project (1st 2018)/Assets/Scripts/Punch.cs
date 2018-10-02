using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {

    [Tooltip("La puissance du coup de poing")] public float punchForce = 375f;

    private bool CanHit = false;
    private PlayerController playerHit;


    private void OnTriggerEnter(Collider other)
    {
        playerHit = other.GetComponent<PlayerController>();        //attrapage de la référence du joueur
        if (playerHit)                                             // si c'est belle et bien un joueur qu'on a collide...
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();                                                                       
            if(transform.parent.gameObject.GetComponent<PlayerController>().teamDemon)  //si je suis un démon
            {
                if(playerHit.teamAngel)            //si le mec que je tabasse est un ange
                {
                    //ALORS PAF LE STUN
                    playerHit.isStunned = true;                             // du coup j'utilise un check dans l'update c'est peut être pas la meilleur manière
                    playerRb.AddForce(transform.up * punchForce * Time.deltaTime, ForceMode.Impulse);

                                                                            // je dois probablement pouvoir passer direct l'info à l'instance du joueur qui s'est fait toucher.
                    if (playerHit.hasTheSoul)                               // et si ce joueur à bien la balle
                    {
                        //CanHit = true;
                        playerHit.hasTheSoul = false;
                        Debug.Log("pique la balle");
                        transform.parent.gameObject.GetComponent<PlayerController>().hasTheSoul = true;
                        //StartCoroutine(DoOnce(other.gameObject));
                    }

                }
            }

            if (transform.parent.gameObject.GetComponent<PlayerController>().teamAngel)  //si je suis un ange
            {
                if (playerHit.teamDemon)            //si le mec que je tabasse est un démon
                {
                    //ALORS PAF LE STUN
                    playerHit.isStunned = true;                             // du coup j'utilise un check dans l'update c'est peut être pas la meilleur manière
                    playerRb.AddForce(transform.up * punchForce * Time.deltaTime, ForceMode.Impulse);

                                                                            // je dois probablement pouvoir passer direct l'info à l'instance du joueur qui s'est fait toucher.
                    if (playerHit.hasTheSoul)                               // et si ce joueur à bien la balle
                    {
                        //CanHit = true;
                        playerHit.hasTheSoul = false;
                        transform.parent.gameObject.GetComponent<PlayerController>().hasTheSoul = true;
                        //StartCoroutine(DoOnce(other.gameObject));
                    }
                }
            }

            

        }
    }

    //IEnumerator DoOnce(GameObject other)
    //{
    //    yield return new WaitForSeconds(1);
    //    CanHit = false;
    //}
}
