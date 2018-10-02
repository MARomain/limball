using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [Header("Le numéro du joueur (utilisé pour les inputs)")]
    public int playerNumber = 1;

    [Tooltip("la couleur du joueur")] public Color playerColor;

    private Color baseColor; //couleur du material, sert pour le stun, temporaire si des FX sont ajouté

    [Header("Si le joueur peut se déplacer ")]
    private float baseSpeed;

    [Header("Déplacements de base")]
    [Tooltip("La vitesse de déplacement de base")] public float speed = 6f;

    [Tooltip("La vitesse de déplacement avec la soul")] public float speedWithSoul = 5f;

    [Tooltip("La vitesse de rotation du joueur")] [Range(0.0f, 1.0f)] public float turnSpeed = 0.15f;
    private float baseTurnSpeed;

    [Header("La passe")]
    [Tooltip("La portée de la passe")] public float passRange = 100f;

    [Tooltip("Le temps avant de pouvoir faire la passe après l'avoir reçu")] public float passCooldown = 1f;

    [Tooltip("Si le joueur peut passer la balle")] public bool passBool = true;

    public float passCooldownTimer;

    [Header("Le punch")]
    [Tooltip("Le gameobject du collider pour le poing/dash")] public GameObject punchGO;

    [Tooltip("CoolDown après avoir utilisé le punch")] public float punchCooldown = 1f;

    [Tooltip("Durée du punch après l'appuie de la touche")] public float punchDuration = 0.3f;

    [Tooltip("Si le joueur peut punch")] public bool punchBool = true;

    public bool punchDurationBool = false;

    public bool punchMovement;

    public float punchDurationTimer = 0.3f;

    public float punchCooldownTimer = 1f;

    [Tooltip("La vitesse de déplacement du personnage quand il punch")] public float speedPunch = 1f;

    [Tooltip("Sa vitesse de rotation")] public float turnSpeedPunch = 0.01f;

    [Header("Le dash")]
    [Tooltip("La vitesse du dash")] public float dashSpeed = 20f;

    [Tooltip("Le cooldown du dash")] public float dashCooldown = 3f;

    public float dashCooldownTimer = 3f;

    public float dashDurationTimer = 0.3f;

    public bool dashDurationBool = false;

    public float dashCount;

    public int nbOfDash = 2;

    public Slider dashSlider;

    public Image dashImage;

    [Tooltip("La durée du dash")] public float dashDuration = 0.3f;

    [Tooltip("S'il est possible de dash")] public bool dashBool = true;

    [Header("Etat de joueur")]

    [Tooltip("Si le joueur peut envoyer des inputs")] public bool enableControls = true;

    [Tooltip("Si le joueur possède la soul (balle)")] public bool hasTheSoul;

    [Tooltip("Si le joueur est stun (punch / dash d'un ennemie)")] public bool isStunned = false;

    [Tooltip("La durée d'un stun")] public float stunDuration = 1.5f;

    public float stunDurationTimer = 1.5f;

    [Tooltip("La couleur du joueur quand il est stun")] public Color stunColor;

    [Header("L'équipe du joueur")]
    public bool teamAngel;      // teamAngel / teamDemon doit peut être être gérer dans le PlayerManager
    public bool teamDemon;
    public Animator animator;

    private string horizontalAxisName;
    private string verticalAxisName;
    private string passButton;
    private string punchButton;
    private string dashButton;
    public float verticalInputValue;
    public float horizontalInputValue;
    public Rigidbody rb;
    private MeshRenderer renderer;
    //private LineRenderer lineRenderer;

    private Soul soul;
    private GameManager gameManager;             // doit probablement pouvoir se passer en privée (flemme de tester si ça crée un bug ça marche actuellement comme ça)
    public Vector3 movement;
    //Vector3 destination;

    private void OnTriggerEnter(Collider other)
    {
       soul = other.GetComponent<Soul>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    // Use this for initialization
    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();           //attrapage d'une référence au gameManager qui va me donner accés à la liste des joueurs pour faire des passes
        rb = GetComponent<Rigidbody>();
            
        // ref a rigidbody de l'objet (déplacements)
        //lineRenderer = GetComponentInChildren<LineRenderer>();

        //différents setup de ref pour les inputs
        verticalAxisName = "Vertical" + playerNumber;
        horizontalAxisName = "Horizontal" + playerNumber;
        passButton = "Pass" + playerNumber;
        punchButton = "Punch" + playerNumber;
        dashButton = "Dash" + playerNumber;

        // initialisation de variables utile dans le code.
        baseSpeed = speed;
        baseTurnSpeed = turnSpeed;
        passBool = true;
        punchBool = true;
        punchMovement = false;

        //set de la couleur du joueur
        dashImage.color = playerColor;       // set la couleur du cercle autour du joueur

        //baseColor = renderer.material.color; // on récupère et stock la couleur de base du player, temporaire en attendant les FX.

    }

    void FixedUpdate ()
    {
        if (enableControls)
        {
            Move();
        }
    }

    private void Update()
    {
        if (enableControls)
        { 
            Pass();
            Punch();
            Dash();
        }
        Stunned();
        ShowPassRange();
        //DashCircleUpdate();
        DashCD();
        PassCD();
        PunchCD();
    }

    private void Stunned()
    {
        if(isStunned)                           //si le joueur est stun
        {
            //renderer.material.color = stunColor;
            animator.SetBool("Stunned", true);

            stunDurationTimer -= Time.deltaTime;

            if(stunDurationTimer <= 0f)
            {
                isStunned = false;
                //renderer.material.color = baseColor;
                animator.SetBool("Stunned", false);
                stunDurationTimer = stunDuration;
            }
        }
    }

    private void ShowPassRange()
    {
        if (hasTheSoul)
        {
            //lineRenderer.enabled = true;
        }
        else
        {
            //lineRenderer.enabled = false;
        }
    }

    private void Move()
    {
        if(!isStunned)
        {
            if (hasTheSoul)                     // si le joueur possède la balle il va moins vite
            {
                speed = speedWithSoul;
            }
            else speed = baseSpeed;

            if (punchMovement)
            {
                speed = speedPunch;
                turnSpeed = turnSpeedPunch;
            }
            else
            {
                speed = baseSpeed;
                turnSpeed = baseTurnSpeed;
            }

            //Debug.Log(Input.GetAxis("Horizontal" + playerNumber));
            verticalInputValue = Input.GetAxis(verticalAxisName);
            horizontalInputValue = Input.GetAxis(horizontalAxisName);

            movement.Set(horizontalInputValue, 0f, verticalInputValue);
            movement = movement.normalized * speed * Time.deltaTime;

            rb.MovePosition(transform.position + movement);

            // ******   Ceci fait que le perso se déplace à la "destination" qui est en world space ******
            // ******   Pas vraiment utile pour le déplacement mais sera utile pour renvoyer un joueur à une position ******

            //destination.Set(verticalInputValue, 0f, horizontalInputValue);
            //movement = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);


            // Manière de faire avec transform.translate

            //transform.Translate(verticalMovement, 0f, horizontalMovement);
            //verticalMovement = verticalInputValue * Time.deltaTime * speed;
            //horizontalMovement = horizontalInputValue * Time.deltaTime * speed;



            //LE FAIT QUE LE PERSO SOIT TOURNER VERS LA OU IL SE DEPLACE
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), turnSpeed);
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
        }
           
    }

    private void Pass()
    {
        if (Input.GetButtonDown(passButton))                                                                                                    // si le joueur appuie sur le bouton de la passe
        {
            Debug.Log("Appuie sur la touche de pass");
            if(!isStunned)                                                                                                                          // s'il est pas stun
            {
                if (hasTheSoul)                                                                                                                     // s'il possède la balle.
                {
                    if (passBool)                                                                                                                   // s'il peut passer la balle (cooldown)
                    {
                        if (teamDemon)                                                                                                              // si je suis un démon
                        {
                            for (int i = 0; i < gameManager.players.Length; i++)                                                                    // on cherche parmis tout les joueurs
                            {
                                if (gameManager.players[i].playerController.teamDemon == true)                                                      // si le pote est lui aussi un démon
                                {
                                    if(gameManager.players[i].playerNumber != playerNumber)
                                        //quand la boucle passe elle va trouver le player 3 comme étant un démon et si c'est le player 3 qui 
                                        // possède la balle. il va essayer de se faire la passe à lui même (CE CON)
                                        //ce if évite ça
                                    {
                                        if(gameManager.players[i].playerController.isStunned == false)
                                        {
                                            if (Vector3.Distance(transform.position, gameManager.players[i].instance.transform.position) < passRange)       //si le pote est à porter
                                            {
                                                gameManager.players[i].playerController.hasTheSoul = true;          // la balle est passé au pote
                                                Debug.Log("passe");
                                                hasTheSoul = false;                                                 // le joueur qui a la balle au moment de la passe ne la possède plus
                                                gameManager.players[i].playerController.passBool = false;            // le mec qui vient de recevoir la balle ne peut plus faire la passe

                                                //gameManager.players[i].playerController.StartCoroutine(CoolDownPass()); //cette ligne devrait fonctionner
                                                //et quand passBool est faux le cooldown est lance (check update c'est pas opti)

                                                //Au lieu de passer direct l'info avec la première ligne commenter. 
                                                // Je check dans Update si passBool est faux et si c'est le cas je lance le cooldown. 
                                                // Mais du coup je fais pleins de checks inutiles

                                                // bug quand t'es le player 1 ou player le player 3 la balle se détache si on est pas à portée la soul se détache au lieu de ne rien faire (return)
                                                // peut être du à la manière dont je passe dans le tableau ?
                                            }
                                            else return;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }


                if (hasTheSoul)                                                                                                                     // s'il possède la balle.
                {
                    if (passBool)                                                                                                                   // s'il peut passer la balle (cooldown)
                    {
                        if (teamAngel)                                                                                                              // si je suis un ange
                        {
                            for (int i = 0; i < gameManager.players.Length; i++)                                                                    // on cherche parmis tout les joueurs
                            {
                                if (gameManager.players[i].playerController.teamAngel == true)                                                      // si le pote est lui aussi un ange
                                {
                                    if (gameManager.players[i].playerController.playerNumber != playerNumber)
                                    //quand la boucle passe elle va trouver le player 1 comme étant un ange, et si c'est le player 1 qui 
                                    //possède la balle. il va essayer de se faire la passe à lui même (CE CON)
                                    //ce if évite ça


                                    {
                                        if (gameManager.players[i].playerController.isStunned == false)
                                        {

                                            if (Vector3.Distance(transform.position, gameManager.players[i].instance.transform.position) < passRange)       //si le pote est à porter
                                            {
                                                gameManager.players[i].playerController.hasTheSoul = true;          // la balle est passé au pote
                                                Debug.Log("passe");
                                                hasTheSoul = false;                                                 // le joueur qui a la balle au moment de la passe ne la possède plus
                                                gameManager.players[i].playerController.passBool = false;            // le mec qui vient de recevoir la balle subit un cooldown
                                                                                                                     //gameManager.players[i].playerController.StartCoroutine(CoolDownPass()); //le cooldown se lance
                                            }
                                            else return;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
           

        }
    }

    private void PassCD()                        //méthode de gitan voir commentaires code pass
    {
        if (!passBool)
        {
            passCooldownTimer -= Time.deltaTime;

            if(passCooldownTimer <= 0f)
            {
                passBool = true;
                passCooldownTimer = passCooldown;
            }
        }
    }


    private void Punch()
    {
        if (Input.GetButtonDown(punchButton))                   // si t'appuie sur le bouton pour punch
        {
            if(!isStunned)                                      //si tu n'es pas stun
            {
                if (!hasTheSoul)                                     // si t'as pas la soul (si t'as la soul t'as pas le droit de punch)
                {
                    if (punchBool)                                  // si t'as le droit de punch (cooldown)
                    {
                        //lancer l'anim en mettant une bool true
                        punchGO.GetComponent<Collider>().enabled = true;        //enable le punch (start d'une animation à l'avenir)
                        //punchGO.GetComponent<MeshRenderer>().enabled = true;
                        punchMovement = true;                                   //passe la vitesse de déplacement du joueur à la vitesse quand il punch (voir haut movement())
                        punchBool = false;
                        punchDurationBool = true;
                        int random = Random.Range(1, 3); // renvoie soit 1 soit 2 
                        Debug.Log(random);
                        if(random == 1)
                        animator.SetBool("RightPunch", true);

                        if(random == 2)
                        animator.SetBool("LeftPunch", true);
                        //je change la mouvement speed du joueurs dans movement quand on frappe, il est très ralentie.




                        //testé ce genre de truc 
                        //targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius); (à prendre dans charlatank)

                    }


                }
            }
        }
    }

    private void PunchCD()
    {
        if(punchDurationBool)
        {
            punchDurationTimer -= Time.deltaTime;
        }

        if(punchDurationTimer <= 0f)
        {
            punchGO.GetComponent<Collider>().enabled = false;           // désactivé le collider du punch
            //punchGO.GetComponent<MeshRenderer>().enabled = false;
            // set la bool de l'anim en false
            punchMovement = false;                                      // la vitesse de déplacement redevient normal  
            
            punchDurationBool = false;
            animator.SetBool("RightPunch", false);
            animator.SetBool("LeftPunch", false);
            punchDurationTimer = punchDuration;
        }

        if (!punchBool)
        {
            punchCooldownTimer -= Time.deltaTime;
        }

        if(punchCooldownTimer <= 0f)
        {
            punchBool = true;
            punchCooldownTimer = punchCooldown;
        }
    }

    private void Dash()
    {
        if(Input.GetButtonDown(dashButton))             // si t'appuies sur le bouton du dash
        {
            if(dashCount > 0) //(if dashBool)
            {
                if (!hasTheSoul)
                {
                    if(!dashDurationBool)                       // s'il n'est pas en train de dash
                    {
                        if (!isStunned)                              // s'il n'est pas stun
                        {
                            rb.AddForce(transform.forward * dashSpeed, ForceMode.Impulse);       //on propulse le joueur
                            punchGO.GetComponent<Collider>().enabled = true;                    // activation du collider (plus tard anim)
                            //punchGO.GetComponent<MeshRenderer>().enabled = true;
                            //dashBool = false;                                                   // interdir de re dash
                            dashCount -= 1;
                            dashDurationBool = true;
                            animator.SetBool("Dashing", true);

                        }
                    }
                }
            }
        }
    }

    private void DashCD()
    {
        if (dashDurationBool)                   // lance le compteur
        {
            dashDurationTimer -= Time.deltaTime;
        }

        if (dashDurationTimer <= 0f)  // a la fin de la durée du compteur stop l'effet du dash
        {
            punchGO.GetComponent<Collider>().enabled = false;                   //après la durée du dash on disable le collider
            //punchGO.GetComponent<MeshRenderer>().enabled = false;               // et le mesh renderer (probablement une anim dans le future ici
            rb.velocity = movement;
            dashDurationBool = false;
            dashDurationTimer = dashDuration;
            animator.SetBool("Dashing", false);
        }

        if  (dashCount < nbOfDash)        //(!dashBool)
        {
            dashCooldownTimer -= Time.deltaTime;        // Lancement du CD
            //dashSlider.value = dashCooldownTimer;       //update UI 
            //dashCount += Time.deltaTime;
        }

        if (dashCooldownTimer <= 0f)
        {
            //dashBool = true;                            //CD UP 
            dashCount += 1;
            dashCooldownTimer = dashCooldown;           // reset du timer
        }

        dashSlider.value = dashCount;
    }


}


    //Utilisé au début du projet pour debug les valeurs des inputs du déplacements avec un print sur l'écran.

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 10, 100, 20), horizontalInputValue.ToString());
    //    GUI.Label(new Rect(10, 30, 100, 20), verticalInputValue.ToString());
    //}