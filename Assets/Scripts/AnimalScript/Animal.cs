using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
   
    [Header("AnimalComponents")]
    private AudioSource audioSource;


    [SerializeField] protected sbyte lifes;
    [SerializeField] protected float baseMovementSpeed;
    [SerializeField] protected float movementSpeed;
    [SerializeField] protected float backwardsSpeed;
    [SerializeField] protected float backwardsLimit = 5;
    [SerializeField] protected int rotationSpeed = 150;
    [SerializeField] protected Vector2 movementDirection = Vector2.left;
    [SerializeField] protected Vector2 backwardPos;
    protected Animator anim;
    [SerializeField] protected AudioClip[] pupySounds;
    private bool animalDead;
    public bool AnimalDead { get => animalDead; }


    private bool throwAnimalBack = false;
    private bool animalKicked = false;
    public bool animalInsideCorral;


    [Header("CorralVars")]
    float xCorralMin;
    float xCorralMax;
    float yCorralMin;
    float yCorralMax;

    [Header("ThrowCorralVars")]
    [SerializeField] private float kickedSpeed;
    [SerializeField] float gravity = 4;
    [SerializeField] float xComponent;
    [SerializeField] float yComponent;
    [SerializeField] float angle;
    [SerializeField] float finalAngle;
    [SerializeField] float angleCos;
    [SerializeField] float angleSin;
    [SerializeField] float flySpeed = 7;
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;

    private void Start()
    {
        movementSpeed = 0.28f;
        anim = GetComponent<Animator>();
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>(); ;
        xCorralMin = GameObject.Find("/Limits/CorralLimits/CorralLeftLimit").transform.position.x;
        xCorralMax = GameObject.Find("/Limits/CorralLimits/CorralRightLimit").transform.position.x;
        yCorralMin = GameObject.Find("/Limits/CorralLimits/CorralBottomLimit").transform.position.y;
        yCorralMax = GameObject.Find("/Limits/CorralLimits/CorralTopLimit").transform.position.y;
    }

    private void Update()
    {
        if(animalDead == false)
        {
            ApplyMovement();
            ThrowAnimalBack();
            AnimalKicked();
        }

        if (lifes <= 0 && animalDead == false)
            animalDead = true;
        

        
    }

    void ApplyMovement()
    {
        if(throwAnimalBack == false && animalKicked == false)
        {
            if (transform.rotation != Quaternion.identity)
                transform.rotation = Quaternion.identity;
            this.transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
        }
    }

    public void AnimalBackTrue()
    {
        throwAnimalBack = true;
        lifes--;
        backwardPos = new Vector2(transform.position.x + backwardsLimit, transform.position.y);
        PlayRandomSound();
    }

    void ThrowAnimalBack()
    {
        if(throwAnimalBack == true && animalKicked == false)
        {
            if(transform.position.x < backwardPos.x)
            {
                transform.Translate(Vector2.right * backwardsSpeed * Time.deltaTime);
            }
            else
            {
                throwAnimalBack = false;
            }
        }
        else
        {
            if(throwAnimalBack == true && animalKicked == true)
            {
                throwAnimalBack = false;
                lifes = 0;
                //animalDeleteado = true;
            }
            
        }
    }

    public void AnimalKickedTrue()
    {
        if(animalKicked == false)
            CalculateAngle();
    }

    void AnimalKicked()
    {
        if(animalKicked == true)
        {
            ApplyKickedMovement();
            RotateAnimal();
        }
    }

    

    float timeSinceAnimalKicked;
    void ApplyKickedMovement()
    {
        timeSinceAnimalKicked += Time.deltaTime;

        xComponent = (kickedSpeed * angleCos * timeSinceAnimalKicked) + startPos.x;
        yComponent =  (float)(kickedSpeed * angleSin * timeSinceAnimalKicked - (gravity * Mathf.Pow(timeSinceAnimalKicked, 2)) / 2) + startPos.y;
        transform.position =  new Vector2(xComponent, yComponent);

        if(transform.position.x > endPos.x)
        {
            timeSinceAnimalKicked = 0;
            animalKicked = false;
            kickedSpeed = 2;
        }
    }

    void RotateAnimal()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }


    private void CalculateAngle()
    {
        startPos = transform.position;
        endPos = new Vector2(Random.Range(xCorralMin, xCorralMax), Random.Range(yCorralMin, yCorralMax));
        Vector2 targetDir = endPos - startPos;
        float y = targetDir.y;
        targetDir.y = 0;
        float x = targetDir.magnitude;
        float sSqr = Mathf.Pow(kickedSpeed, 2);
        float underTheSqueareRoot = Mathf.Pow(kickedSpeed, 4) - gravity * (gravity * Mathf.Pow(x, 2) + 2 * y * sSqr);
        
        if(underTheSqueareRoot >= 0)
        {
            float root = Mathf.Sqrt(underTheSqueareRoot);
            float angle = sSqr + root;
            float finalAngle = Mathf.Atan2(sSqr + root, gravity * x);
            angleCos = Mathf.Cos(finalAngle);
            angleSin = Mathf.Sin(finalAngle);
            animalKicked = true;
            PlayRandomSound();
        } else
        {
            kickedSpeed++;
            CalculateAngle();
        }
    }

    void PlayRandomSound()
    {
        audioSource.PlayOneShot(pupySounds[Random.Range(0, pupySounds.Length)], audioSource.volume);
    }
    

}
