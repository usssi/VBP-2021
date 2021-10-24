using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadMovement : MonoBehaviour
{
    [Header("ChadComponents")]
    private Animator anim;
    private ChadSoundsManager soundsManager;
    private BoxCollider2D bColl2d;

    [Header("MovementVariables")]
    [SerializeField] float movementSpeed;
    [SerializeField]private bool canMove = true;
    float hMovement, vMovement;
    public float HMovement { get { return hMovement; } }
    public float VMovement { get { return vMovement; } }
    float joystickDeadRange = 0.2f;

    [Header("JumpVariables")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float timeMultiplier;
    float maxJumpSpeed, minJumpSpeed;
    bool jumping, falling;
    public bool Jumping { get { return jumping; } }
    public bool Falling { get { return falling; } }
    private bool inAir;
    public bool InAir { get => inAir; }
    private Vector2 jumpStart;
    Vector2 jumpDirection = new Vector2(1, 2);
    Vector2 fallDirection = new Vector2(1, -2);

    [Header("BlowVars")]
    private bool canBlow;
    public bool hasBlow;

    [Header("KickVars")]
    [SerializeField] private float kickCooldown;
    private float kickCronometer;
    [SerializeField] private float radiusDetection;
    private GameManager gameManager;

    [Header("Buttons")]
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject jumpButton;
    [SerializeField] GameObject blowButton;
    [SerializeField] GameObject kickButton;

    //[Header("Sounds")]
    //[SerializeField] private AudioClip chadAttack, chadJump;

    private void Start()
    {
        maxJumpSpeed = jumpSpeed;
        //soundsManager = GetComponent<ChadSoundsManager>();
        anim = GetComponent<Animator>();
        //animalGetter = GameObject.FindGameObjectWithTag("AnimalGetter").GetComponent<AnimalGetter>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        bColl2d = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        Kick();

        if(inAir == true)
        {
            if(kickButton.activeSelf == true)
                kickButton.gameObject.SetActive(false);

            if (blowButton.activeSelf == false)
                blowButton.gameObject.SetActive(true);
        }
        else
        {
            if (kickButton.activeSelf == false)
                kickButton.gameObject.SetActive(true);

            if (blowButton.activeSelf == true)
                blowButton.gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        Movemement();
        Jump();
    }

    void Movemement()
    {
        if (inAir == false && canMove == true)
        {
            if (joystick.Horizontal > joystickDeadRange)
                hMovement = 1;
            else if (joystick.Horizontal < -joystickDeadRange)
                hMovement = -1;
            else
                hMovement = 0;


            if (joystick.Vertical > joystickDeadRange)
                vMovement = 1;
            else if (joystick.Vertical < -joystickDeadRange)
                vMovement = -1;
            else
                vMovement = 0;

            Vector2 direction = new Vector2(hMovement, vMovement).normalized;
            Vector2 move = direction * movementSpeed * Time.deltaTime;

            transform.Translate(move);
        }

    }

    void Jump()
    {
        if (inAir == true)
        {
            if (jumping == true && falling == false)
            {
                jumpSpeed = Mathf.Clamp(jumpSpeed - Time.deltaTime * timeMultiplier, minJumpSpeed, maxJumpSpeed);
                transform.Translate(jumpDirection * jumpSpeed * Time.deltaTime);
                if (jumpSpeed <= 0)
                {
                    jumping = false;
                    falling = true;
                }
            } else if (jumping == false && falling == true)
            {
                jumpSpeed = Mathf.Clamp(jumpSpeed + Time.deltaTime * timeMultiplier, minJumpSpeed, maxJumpSpeed);
                transform.Translate(fallDirection * jumpSpeed * Time.deltaTime);
                if (transform.position.y <= jumpStart.y)
                {
                    falling = false;
                    inAir = false;
                    jumpSpeed = maxJumpSpeed;

                    bColl2d.isTrigger = false;

                }
            }
        }
    }

    

    public void JumpButton()
    {
        if (inAir == false && jumping == false && falling == false)
        {
            bColl2d.isTrigger = true;
            //soundsManager.PlaySound(chadJump);
            FindObjectOfType<AudioManager>().Play("saltoChad");

            inAir = true;
            jumping = true;
            jumpStart = transform.position;
            canBlow = true;
        }
    }

    public void BlowButton()
    {
        if (canBlow == true && jumpSpeed < 2f)
        {
            //soundsManager.PlaySound(chadAttack);
            FindObjectOfType<AudioManager>().Play("golpeChad");

            anim.SetTrigger("blow");
            if (jumping == true)
            {
                jumpSpeed = 0;
            }
            hasBlow = true;
            canBlow = false;
            StartCoroutine(WaitForMakeBlowFalse());
        }

    }

    public void KickButton()
    {
        if (kickCronometer <= 0)
        {
            if (inAir != true)
            {
                for (int i = 0; i < gameManager.GetAnimalList().Count; i++)
                {
                    if (CheckIfAnimalNear(gameManager.GetAnimalList()[i]))
                    {
                        AnimalState animScript = gameManager.GetAnimalList()[i].GetComponent<AnimalStateMachine>().CurrentState;

                        animScript.AnimalKicked();
                        kickCronometer = kickCooldown;
                        anim.SetTrigger("kick");
                        FindObjectOfType<AudioManager>().Play("chadPatada");


                    }
                }

            }
        }
    }


    void Kick()
    {
        if (kickCronometer > 0)
        {
            kickCronometer -= Time.deltaTime;
        }
    }

    bool CheckIfAnimalNear(GameObject ani)
    {
        if(ani != null)
        {
            if (Vector2.Distance(ani.transform.position, transform.position) < radiusDetection)
            {
                return true;
            }

        }

        return false;
    }

    public void SetJoystick(Joystick _joystick)
    {
        joystick = _joystick;
    }


    IEnumerator WaitForMakeBlowFalse()
    {
        yield return new WaitForSeconds(0.1f);

        hasBlow = false;
    }


    public void SetDifficulty(float _newMovSpd, float _newJumpSpd, float _newTimeMult, float _newKickCd)
    {
        movementSpeed = _newMovSpd;
        maxJumpSpeed = _newJumpSpd;
        jumpSpeed = _newJumpSpd;
        kickCooldown = _newKickCd;
        timeMultiplier = _newTimeMult;
    }


    
}
