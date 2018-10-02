using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zkill : MonoBehaviour
{

    private GameManager gameManager;
    private PlayerController player;
    private Soul soul;
    private bool startTimer;
    private float floatTimer;
    private AudioSource audio;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audio = GetComponent<AudioSource>();
        floatTimer = gameManager.startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            floatTimer -= Time.deltaTime;
        }

        if (floatTimer <= 0f)
        {
            player.animator.SetBool("Flying", false);
            //player.enableControls = true;
            floatTimer = gameManager.startDelay;
            startTimer = false;
            Debug.Log("passage");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();
        //soul = GameObject.Find("Soul(Clone)").GetComponent<Soul>();
        if (player)
        {
            player.gameObject.SetActive(false);
            //player.enableControls = false;
            player.hasTheSoul = false;
            player.rb.velocity = Vector3.zero;
            player.transform.position = gameManager.players[player.playerNumber - 1].spawnTransform.position;
            player.transform.rotation = gameManager.players[player.playerNumber - 1].spawnTransform.rotation;
            player.gameObject.SetActive(true);
            audio.Play();
            startTimer = true;
            player.animator.SetBool("Flying", true);
        }
    }
    //private IEnumerator EnableControl()
    //{
    //    yield return new WaitForSeconds(gameManager.startDelay);
    //    player.enableControls = true;
    //}
}



//private GameManager gameManager;
//private PlayerController player;
//private Soul soul;
//private bool startTimer;
//private float floatTimer;
//private AudioSource audio;
//// Use this for initialization
//void Start()
//{
//    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//    audio = GetComponent<AudioSource>();
//    floatTimer = gameManager.startDelay;
//}

//// Update is called once per frame
//void Update()
//{
//    if (startTimer)
//    {
//        floatTimer -= Time.deltaTime;
//    }

//    if (floatTimer <= 0f)
//    {
//        player.animator.SetBool("Flying", false);
//        //player.enableControls = true;
//        floatTimer = gameManager.startDelay;
//        startTimer = false;
//        Debug.Log("passage");
//    }
//}

//private void OnTriggerEnter(Collider other)
//{
//    player = other.GetComponent<PlayerController>();
//    //soul = GameObject.Find("Soul(Clone)").GetComponent<Soul>();
//    if (player)
//    {
//        player.gameObject.SetActive(false);
//        //player.enableControls = false;
//        player.hasTheSoul = false;
//        player.rb.velocity = Vector3.zero;
//        player.transform.position = gameManager.players[player.playerNumber - 1].spawnTransform.position;
//        player.transform.rotation = gameManager.players[player.playerNumber - 1].spawnTransform.rotation;
//        player.gameObject.SetActive(true);
//        audio.Play();
//        startTimer = true;
//        player.animator.SetBool("Flying", true);
//    }