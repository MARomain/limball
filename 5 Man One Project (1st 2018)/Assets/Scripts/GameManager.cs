using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {


    public float startDelay = 1f;
    public float endDelay = 1f;
    public float endGameDelay = 2f;
    public GameObject angelPrefab;
    public GameObject demonPrefab;
    public GameObject soulPrefab;
    public TextMeshPro endRoundText;
    public PlayerManager[] players;
    public SoulManager soul;
    public int teamSize = 2;

    public Color[] playerColors;    // cette variable est uniquement présente dans le game manager car je set les couleurs via l'inspector. 
                                    // elle devrait être dans le playerManager et je devrai sauvegarder 4 variables de couleurs j'imagine

    public GoalAngel goalAngel;
    public GoalDemon goalDemon;     // je pense que ces trois éléments je peux les passer en private puis après en faisant un truc style getcomponent 
    public Timer timer;             // mais actuellement avec un simple get componenent ça ne marche pas donc je drag depuis l'inspector mes éléments.

    public AudioSource audio;
    public AudioClip but;
    public AudioClip finDeMatch;
    public AudioClip debutDeMatch;
    private WaitForSeconds startWait;
    private WaitForSeconds endRoundWait;
    private WaitForSeconds endGameWait;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1f;                            //au cas ou à cause du menu pause
        startWait = new WaitForSeconds(startDelay);     // délay avant le début d'un round
        endRoundWait = new WaitForSeconds(endDelay);         // délay après un but
        endGameWait = new WaitForSeconds(endGameDelay);

        audio = GetComponent<AudioSource>();
        //goalDemon = GetComponent<GoalDemon>();
        //goalAngel = GetComponent<GoalAngel>();
        //timer = GetComponent<Timer>();

        SpawnPlayers();                                 // spawn de tout les joueurs
        StartCoroutine(GameLoop());                     // Début de la game loop et donc de la partie.
	}


    private void SpawnPlayers()
    {
        for (int i = 0; i < players.Length; i++)                //on va chercher tout les joueurs
        {
            //lors de la première boucle, playernumber = i+1 donc 0+1 et ainsi dessuite.
            players[i].playerNumber = i+1;
            players[i].playerColor = playerColors[i];
            if (players[i].playerNumber <= teamSize)            //on assigne la moitié des joueurs dans une équipe
            {
                players[i].instance = Instantiate(angelPrefab, players[i].spawnTransform.position, players[i].spawnTransform.rotation) as GameObject;
                players[i].teamAngel = true; //est-ce que je dois mettre "teamAngel" dans le playerController ou je peux le mettre dans le PlayerManager ?
            }
            else// et le reste dans l'autre équipe
            {
                players[i].instance = Instantiate(demonPrefab, players[i].spawnTransform.position, players[i].spawnTransform.rotation) as GameObject;
                players[i].teamDemon = true;
            }

            players[i].Setup();         //passage de variable : playerNumber / team

        }
    }

    private void SpawnSoul()        //Spawn de la soul (balle)
    {
        soul.instance = Instantiate(soulPrefab, soul.spawnPointSoul.position, soul.spawnPointSoul.rotation) as GameObject;
    }


    private IEnumerator GameLoop()      //La déroulement de la partie prend place dans cette GameLoop
    {
        yield return StartCoroutine(RoundStarting());

        yield return StartCoroutine(RoundPlaying());

        yield return StartCoroutine(RoundEnding());

        StartCoroutine(GameLoop());
    }

    private IEnumerator RoundStarting() //initialisation du round
    {
        ResetPlayers();   //lance aussi l'anim de chutte des joueurs              // on reset les joueurs
        SpawnSoul();                    // fait apparaitre une soul (balle)
        ResetWhoScored();               // on reset les variables de chaque cage sur qui a marqué un but le round précédent                       
        DisablePlayerControl();         // les joueurs ne peuvent pas bouger
        endRoundText.text = null;       // réinitialisation du texte de fin (team1 wins etc...)
        Debug.Log("round starting");
        yield return startWait;         // à la fin du temps de startWait on retourne dans la gameLoop
    }

    private IEnumerator RoundPlaying()  // le round commence : FIGHT!
    {
        audio.clip = debutDeMatch;
        audio.Play();

        ResetAnimGameStart();
        EnablePlayerControl();          // les joueurs prennent le controle de leurs personnages
        timer.timerOn = true;           // le timer commence
        Debug.Log("round playing");

        // tant qu'il n'y a pas de but ou que le timer n'arrive pas à 0
        while(soul.instance != null)                    // Ca j'ai essayé d'en faire une fonction qui retourne un bool mais j'ai des soucis. C'est la fonction goalmade()
        {
            if (timer.gameTime <= 0.2f)
            {
                timer.timerOn = false;
                Debug.Log("timer end");
                StartCoroutine(GameEnd());
            }
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisablePlayerControl();         // interdir le controle des joueurs
        timer.timerOn = false;
        Debug.Log("round ending");

        endRoundText.text = null;       // réinitialisation du message texte de fin (team 1 wins etc...)

        if (WhoScored() == 1)
            endRoundText.text = "But des démons";
        else if (WhoScored() == 2)
            endRoundText.text = "But des anges";

        audio.clip = but;
        audio.Play();
        //Actuellement GameEnd() ne fait rien. elle devrait se trigger uniquement quand il n'y a plus de temps au niveau du timer
        // Le timer fait sortir de la RoundPlaying loop et l'amène ici. 
        // On est ici uniquement si un des deux joueurs a mis un goal ou si le timer s'est arrêté

        //si un des joueurs mets un but une action est joué : ici update du texte
        //on peut faire jouer toute sorte de chose, des plans de cams avec animation / son / particules whatever
        // de même pour la fin de la partie

        // il faudra donc une fonction pour définir qui a gagné la partie. 
        // a la fin du temps ou avec un nombre de points max
        // attention cependant le setup actuel supporte uniquement la fin par timer. une nombre de points max devra être implémenter



        yield return endRoundWait;
    }



    //Check si un but à été 
    private bool goalmade()
    {
        // return true si la soul est active
        //return soul.instance.activeself;
        if (soul.instance = null)
        {
            return false;
        }
        else return true;
            
    }

    private int WhoScored()                             // Savoir qu'elle équipe à marquer (devrait être un bool ?)
    {
        if (goalAngel.scoredOnAngelGoal == true)
        {
            return 1;
        }
        else if (goalDemon.scoredOnDemonGoal == true)
        {
            return 2;
        }
        else return 3;
    }


    private IEnumerator GameEnd()
    {
        if (AngelWon())
        {
            timer.timerOn = false;                          //on stop le timer
            endRoundText.text = "Les anges ont gagné";
        }
        else if (DemonWon())
        {
            timer.timerOn = false;                          //on stop le timer
            endRoundText.text = "Les démons ont gagné";
        }
        else
        {
            endRoundText.text = "BUT EN OR !";
            yield return StartCoroutine(ButEnOr());
        }
        audio.clip = finDeMatch;
        audio.Play();
        StartCoroutine(GameEndDelay());
 

    }
    
    private bool AngelWon()
    {
        if (goalDemon.numGoalDemon > goalAngel.numGoalAngel) //si le nombre de but que se sont pris les démons est supérieur au nombre de but que se sont pris les anges.
        {
            return true;
        }
        else return false;
    }

    private bool DemonWon()
    {
        if (goalAngel.numGoalAngel > goalDemon.numGoalDemon) //si le nombre de but que se sont pris les anges est supérieur au nombre de but que se sont pris les démons.
        {
            return true;
        }
        else return false;
    }

    private IEnumerator ButEnOr()
    {
        while(goalAngel.numGoalAngel == goalDemon.numGoalDemon)
        {
            timer.gameTime += Time.deltaTime;
            Debug.Log("rentre dans la loop");
            yield return null;
        }
        StartCoroutine(GameEnd());
        Debug.Log("je sors de ma loop");

    }

    private IEnumerator GameEndDelay()
    {

        Debug.Log("retour au menu");
        yield return endGameWait;
        SceneManager.LoadScene("Menu");
    }



    private void EnablePlayerControl()                  // active le controle des joueurs 
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].EnableControl();
        }
    }

    private void DisablePlayerControl()                 // désactive le control des joueurs
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].DisableControl();
        }
    }

    private void ResetPlayers()                         // reset les joueurs à leurs points de spawn 
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].Reset();
        }
    }

    private void ResetAnimGameStart()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].ResetAnimGameStart();
        }
    }

    private void ResetSoul()                            
    {
        soul.Reset();                                   // reset la position de la soul 
    }

    


        //
        //
        //gros points d'interrogations sur le "else return 3"
        //j'ai d'abord utilisé une fonction "bool" mais j'ai ici 3 cas 
        //Un goal a été marqué chez les ange
        //Un goal a été marqué chez les démons
        //Le Timer s'est arrêté 
        // Timer qui est arrêté qui finalement ne sertait pas trigger par cette fonction, elle est uniquement appelé dans la partie RoundEnding() de la GameLoop()


    private void ResetWhoScored()
    {
        goalDemon.scoredOnDemonGoal = false;    // y'a probablement une manière plus élégante de reset les deux variables qui sont testé dans la fonction au dessus.
        goalAngel.scoredOnAngelGoal = false;
    }



}




// Tout ce code ci dessous est l'archive de ce que j'ai expérimenté à la main en m'inspirant du code de Charlatank
// J'ai essayé de tout contrôlé à l'intérieur de Game manager avec un array d'instance de gameobject et de playercontroller
// Le problème est que pour update le "playernumber" (chiffre qui permet d'avoir les inputs des manettes pour plusieurs joueurs)
// je n'ai pas trouvé de manière avec ce méthode car dans ma boucle (fin de commentaire dans SpawnPlayers) j'update bien le playerNumber
// mais il passe d'abord à 1 puis à 2 pour tout les playerController. Il ne sont pas séparé. 
// Je ne vois pas de manières pour les séparer actuellement. 
// La méthode originel dans charlatank (venant d'un tuto offi unity) me parait bien meilleur. Au final on encapsule toute la partie joueur dans un script.


// -----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------------


//// Use this for initialization
//void Start()
//{

//    //get some references
//    for (int i = 0; i < playerInstance.Length; i++)
//    {
//        playerController[i] = playerInstance[i].GetComponent<PlayerController>();
//    }

//    startWait = new WaitForSeconds(startDelay);
//    endWait = new WaitForSeconds(endDelay);

//    SpawnPlayers();

//    StartCoroutine(GameLoop());
//}

//private void SpawnPlayers()
//{
//    for (int i = 0; i < playerInstance.Length; i++)
//    {
//        playerInstance[i] = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation);
//        playerController[i].playerNumber += 1;
//        Debug.Log(playerController[i].playerNumber);
//    }
//}
