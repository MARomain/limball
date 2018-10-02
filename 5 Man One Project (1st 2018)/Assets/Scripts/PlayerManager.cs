using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class PlayerManager  {



    //Je peux peut être automatiser la collecte des points de spawns ? 
    // actuellement il faut les setup dans le game manager à la main. 
    // mais en soit je pourrai faire un gameobject.find("Spawn1") 2/3/4 etc. 
    // y a t-il pas un autre moyen ? 
    // voir si c'est nécéssaire pour le choix entre IA ou humain ?
    // qu'elle serai le bénéfice de cette automatisation pour le projet ? 














    public Transform spawnTransform;                              // les points de spawns
    [HideInInspector] public int playerNumber;                  // le numéro du joueur qui va être incrémenter durant le spawn et ensuite passer au playerController dans la fonction Setup()
    [HideInInspector] public GameObject instance;               // sert au moment de créer l'instance du gameobject quand on le spawn sinon avec PlayerManager[]
    public Color playerColor;
    public bool teamAngel;
    public bool teamDemon;

    [HideInInspector] public PlayerController playerController;                  // ref au playerController

    // Use this for initialization
    public void Setup ()        //passage d'info au playerController
    {          
        playerController = instance.GetComponent<PlayerController>();

        playerController.playerNumber = playerNumber;
        playerController.teamAngel = teamAngel;
        playerController.teamDemon = teamDemon;
        playerController.playerColor = playerColor;
	}
	

    public void DisableControl()                //Enlève le control des joueurs
    {
        playerController.enableControls = false;
    }

    public void EnableControl()                 // Permet le control des joueurs
    {
        playerController.enableControls = true;
    }

    public void Reset()                         // Reset la position des joueurs
    {
        instance.transform.position = spawnTransform.position;
        instance.transform.rotation = spawnTransform.rotation;
        playerController.hasTheSoul = false;
        playerController.dashCount = playerController.nbOfDash;
        playerController.animator.SetBool("Flying", true);
    }

    public void ResetAnimGameStart()
    {
        playerController.animator.SetBool("Flying", false);
    }

}
