using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour {

    GameObject collidedGO;
    public Vector3 soulOffset;


    public float smoothFollow = 50f;
    private Vector3 velocityFollow;
    public PlayerController player;
    public bool executeSeulementApresAvoirCollideSALE;
    private GameManager gameManager;
    private MeshRenderer renderer;
    private Vector3 desiredPosition;
    public bool collidedOnce = false;
    public GameObject circleGO;
    public GameObject particleAngel;
    public GameObject particleDemon;

    public GameObject particlePlayer1;
    public GameObject particlePlayer2;
    public GameObject particlePlayer3;
    public GameObject particlePlayer4;

    //sert pour le cercle
    public Vector3 playerMovement;
    public Transform playerTransform;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();
        if (player)
        {
            if(!collidedOnce)
            {
                //transform.position = other.transform.position + new Vector3(0, 1, 0); // le mettre 1 au dessus du player qui attrape la balle
                //collidedGO = other.gameObject;
                player.hasTheSoul = true;
                Debug.Log("collision soul");
                executeSeulementApresAvoirCollideSALE = true;
                collidedOnce = true;
            }
        }
    }

    private void SoulOnPlayerHead () // keep the soul following the player
    {
        if (executeSeulementApresAvoirCollideSALE)   // Donne une erreur car il commence à check (player.hasTheSoul) alors que la référence au player 
                                                    // n'a as encore été assigné (premiere ligne OnTriggerEnter
                                                    // C'est sale, à voir comment faire autrement.
        {
            // = collidedGO.transform.position + soulOffset;

            for (int i = 0; i < gameManager.players.Length; i++)
            {
                if (gameManager.players[i].playerController.hasTheSoul == true)
                {
                    desiredPosition = gameManager.players[i].instance.transform.position + soulOffset;

                    //pour le cercle de la passe
                    playerMovement = gameManager.players[i].playerController.movement;
                    playerTransform = gameManager.players[i].instance.transform;

                    //if (gameManager.players[i].playerController.teamAngel)
                    //{
                    //    particleAngel.SetActive(true);
                    //    particleDemon.SetActive(false);
                    //    circleGO.GetComponent<Renderer>().material.color = new Color(1f, 0.92f, 0.016f, 0.4f);
                    //}

                    //if (gameManager.players[i].playerController.teamDemon)
                    //{
                    //    particleDemon.SetActive(true);
                    //    particleAngel.SetActive(false);
                    //    circleGO.GetComponent<Renderer>().material.color = new Color(1f, 0f, 0f, 0.4f);
                    //}

                    if(gameManager.players[i].playerController.playerNumber == 1)
                    {
                        circleGO.GetComponent<Renderer>().material.color = gameManager.players[i].playerController.playerColor;
                        //particleAngel.SetActive(true);
                        //particleDemon.SetActive(false);

                        particlePlayer1.SetActive(true);
                        particlePlayer2.SetActive(false);
                        particlePlayer3.SetActive(false);
                        particlePlayer4.SetActive(false);
                    }

                    if (gameManager.players[i].playerController.playerNumber == 2)
                    {
                        circleGO.GetComponent<Renderer>().material.color = gameManager.players[i].playerController.playerColor;
                        //particleAngel.SetActive(true);
                        //particleDemon.SetActive(false);

                        particlePlayer1.SetActive(false);
                        particlePlayer2.SetActive(true);
                        particlePlayer3.SetActive(false);
                        particlePlayer4.SetActive(false);
                    }

                    if (gameManager.players[i].playerController.playerNumber == 3)
                    {
                        circleGO.GetComponent<Renderer>().material.color = gameManager.players[i].playerController.playerColor;
                        //particleDemon.SetActive(true);
                        //particleAngel.SetActive(false);

                        particlePlayer1.SetActive(false);
                        particlePlayer2.SetActive(false);
                        particlePlayer3.SetActive(true);
                        particlePlayer4.SetActive(false);
                    }

                    if (gameManager.players[i].playerController.playerNumber == 4)
                    {
                        circleGO.GetComponent<Renderer>().material.color = gameManager.players[i].playerController.playerColor;
                        //particleDemon.SetActive(true);
                        //particleAngel.SetActive(false);

                        particlePlayer1.SetActive(false);
                        particlePlayer2.SetActive(false);
                        particlePlayer3.SetActive(false);
                        particlePlayer4.SetActive(true);
                    }
                }
            }

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFollow * Time.deltaTime);
                //Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocityFollow, smoothFollow);

                transform.position = smoothedPosition;
            // transform.position = Vector3.MoveTowards(transform.position, desiredPosition, step);
            //transform.position = desiredPosition;

            //active le cercle
            circleGO.SetActive(true);
            //if(player.teamAngel)

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SoulOnPlayerHead();
    }


}
